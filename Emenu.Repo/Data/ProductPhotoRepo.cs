using Emenu.Dto.Photo;
using Emenu.Dto.Product;
using Emenu.Dto.ProductPhoto;

namespace Emenu.Repo.Data
{
    public class ProductPhotoRepo: EmenuRepository, IProductPhoto
    {
        public ProductPhotoRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<ProductPhotoDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take)
        {
            var result = new OperationResult<HttpStatusCode, List<ProductPhotoDto>>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<ProductPhotoEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("no photos found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }
                //start build quary
                var quary = Context.productPhotos.Where(e => e.IsValid).GroupBy(e => e.ProductId);

                //filter 
                if (!string.IsNullOrEmpty(filter))
                {
                    quary = quary.Where(e => e.Any(s=>s.Product.NameEn.Contains(filter)) || e.Any(s => s.Product.NameAr.Contains(filter)) || e.Any(s => s.Product.Description.Contains(filter)) );
                }
                //applaying skip,take
                result.Result = quary.Skip(skip).Take(take)
                .Select(group => new ProductPhotoDto()
                {
                    product = group.Select(c => new ProductToShowDto()
                    {
                        id = c.Product.Id,
                        nameAr = c.Product.NameAr,
                        nameEn = c.Product.NameEn,
                        description = c.Product.Description,
                        mainPhotoUrl = c.Product.MainPhoto.URL,
                        photoes = Context.productPhotos.Where(e => e.IsValid && e.ProductId == c.ProductId).Select(phto => new PhotoDto() { id = phto.Photo.Id, url = phto.Photo.URL }).ToList(),
                    }).First()

                }).ToList();
                //applaying sort
                if (isDesending)
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "nameen":
                                result.Result = result.Result.OrderByDescending(e => e.product.nameEn).ToList();
                                break;
                            case "namear":
                                result.Result = result.Result.OrderByDescending(e => e.product.nameAr).ToList();
                                break;
                            case "description":
                                result.Result = result.Result.OrderByDescending(e => e.product.description).ToList();
                                break;
                            default:
                                result.Result = result.Result.OrderByDescending(e => e.product.id).ToList();
                                break;
                        }
                    }
                    else
                    {
                        result.Result = result.Result.OrderByDescending(e => e.product.id).ToList();
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "nameen":
                                result.Result = result.Result.OrderBy(e => e.product.nameEn).ToList();
                                break;
                            case "namear":
                                result.Result = result.Result.OrderBy(e => e.product.nameAr).ToList();
                                break;
                            case "description":
                                result.Result = result.Result.OrderBy(e => e.product.description).ToList();
                                break;
                            default:
                                result.Result = result.Result.OrderBy(e => e.product.id).ToList();
                                break;
                        }
                    }
                    else
                    {
                        result.Result = result.Result.OrderBy(e => e.product.id).ToList();
                    }

                }
               
                result.EnumResult = HttpStatusCode.OK;
                await trans.CommitAsync();
            }
            catch (Exception e)
            {
                await trans.RollbackAsync();
                result.AddError(e.Message);
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion

        #region Set
        public async Task<OperationResult<HttpStatusCode, bool>> SetProductPhotoAsync(AddProductPhotoDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                #region Validation
                if (dto.productId == 0 || dto.photos == null)
                {
                    await trans.RollbackAsync();
                    result.AddError("Some Fields are empty");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion


                bool isAdd = dto.id == 0;
                if (isAdd)
                {
                    var photoSet = await Context.productPhotos.Where(we => we.IsValid && we.ProductId == dto.productId).ToListAsync();
                    foreach (var item in photoSet)
                    {
                        item.IsValid = false;
                    }
                    Context.productPhotos.UpdateRange(photoSet);
                    Context.SaveChanges();
                }
           
                List<ProductPhotoEntity > productPhotoEntities= new List<ProductPhotoEntity>();
                bool exsist;

                if (isAdd && (dto.photos == null || dto.photos.Count == 0))
                {
                    await trans.RollbackAsync();
                    result.AddError("Please select one photo at least");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
            

                foreach (var item in dto.photos)
                {
                     exsist = await CheckEntityExsist<PhotoEntity>(e => e.IsValid && e.URL ==item);
                    if (!exsist) 
                    {
                        await Context.photos.AddAsync(new PhotoEntity() {URL=item });
                        Context.SaveChanges();
                    }

                    productPhotoEntities.Add(new ProductPhotoEntity()
                    {
                        PhotoId=Context.photos.First(e=>e.IsValid && e.URL==item).Id,
                        ProductId=dto.productId,
                        
                    });
                }
                if (isAdd)
                    await Context.productPhotos.AddRangeAsync(productPhotoEntities);
                else
                {
                    foreach (var item in productPhotoEntities)
                    {
                        item.Id = dto.id;
                    }
                    Context.productPhotos.UpdateRange(productPhotoEntities);
                }
                await Context.SaveChangesAsync();
                result.Result = true;
                result.EnumResult = HttpStatusCode.OK;
                await trans.CommitAsync();
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                result.Result = false;
                result.AddError("Interval  error");
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion
    }
}
