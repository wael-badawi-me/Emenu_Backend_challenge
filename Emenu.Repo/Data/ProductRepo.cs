using Emenu.Dto.Photo;
using Emenu.Dto.Product;

namespace Emenu.Repo.Data
{
    public class ProductRepo: EmenuRepository, IProduct
    {
        public ProductRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<ProductDto>>> GetAll(string filter,string sortingCol,bool isDesending,int skip,int take)
        {
            var result = new OperationResult<HttpStatusCode, List<ProductDto>>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<ProductEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("Product not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }
                // build quary
                var quary = Context.products.Where(e => e.IsValid);
                // applaying filter
                if (!string.IsNullOrEmpty(filter))
                {
                    quary = quary.Where(e => e.NameEn.Contains(filter) || e.NameAr.Contains(filter) || e.Description.Contains(filter));
                }
                // applaying sort
                if (isDesending)
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "nameen":
                                quary = quary.OrderByDescending(e => e.NameEn);
                                break;
                            case "namear":
                                quary = quary.OrderByDescending(e => e.NameAr);
                                break;
                            case "description":
                                quary = quary.OrderByDescending(e => e.Description);
                                break;
                            default:
                                quary = quary.OrderByDescending(e => e.CreationDate);
                                break;
                        }
                    }
                    else
                    {
                        quary = quary.OrderByDescending(e => e.CreationDate);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "nameen":
                                quary = quary.OrderBy(e => e.NameEn);
                                break;
                            case "namear":
                                quary = quary.OrderBy(e => e.NameAr);
                                break;
                            case "description":
                                quary = quary.OrderBy(e => e.Description);
                                break;
                            default:
                                quary = quary.OrderBy(e => e.CreationDate);
                                break;
                        }
                    }
                    else
                    {
                        quary = quary.OrderBy(e => e.CreationDate);
                    }

                }
                // applaying skip,take
                result.Result = quary.Skip(skip).Take(take).Select(e => new ProductDto()
                {
                    id = e.Id,
                    description = e.Description,
                    mainPhotoUrl=e.MainPhoto.URL,
                    nameAr=e.NameAr,
                    nameEn=e.NameEn

                }).ToList();
                result.EnumResult = HttpStatusCode.OK;
                await trans.CommitAsync();
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                result.AddError("error");
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion

        #region Set
        public async Task<OperationResult<HttpStatusCode, bool>> SetProductAsync(ProductDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {

                #region Validation 
                if (string.IsNullOrEmpty(dto.nameAr) || string.IsNullOrEmpty(dto.nameEn))
                {
                    result.AddError("please Enter Product Name");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                if (string.IsNullOrEmpty(dto.description))
                {
                    result.AddError("please Enter Product Description");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion
                var exsist = await CheckEntityExsist<PhotoEntity>(e => e.IsValid && e.URL==dto.mainPhotoUrl);
                ProductEntity Product = new ProductEntity();
                Product.NameAr = dto.nameAr;
                Product.NameEn = dto.nameEn;
                Product.Description = dto.description;
                if (!exsist) 
                {
                    var isAdded = await SetPhotoAsync(new PhotoDto() { url = dto.mainPhotoUrl });
                    if (!isAdded.Result)
                    {
                        result.AddError("Retry again");
                        result.Result = false;
                        return result;
                    }
                }
                Product.MainPhotoId = Context.photos.First(c => c.IsValid && c.URL == dto.mainPhotoUrl).Id;

                bool isAdd = dto.id == 0;

                if (isAdd)
                    await Context.products.AddAsync(Product);
                else
                {
                    Product.Id = dto.id;
                    Context.products.Update(Product);
                }
             

                await Context.SaveChangesAsync();
                result.Result = true;
                result.EnumResult = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Result = false;
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }




        private async Task<OperationResult<HttpStatusCode, bool>> SetPhotoAsync(PhotoDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {

                #region Validation 
                if (string.IsNullOrEmpty(dto.url))
                {
                    result.AddError("please Select Photo");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion
                bool isAdd = dto.id == 0;
                PhotoEntity Photo = new PhotoEntity();
                Photo.URL = dto.url;
                if (isAdd)
                    await Context.photos.AddAsync(Photo);
                else
                    Context.photos.Update(Photo);

                await Context.SaveChangesAsync();
                result.Result = true;
                result.EnumResult = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Result = false;
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion

        #region Delete
        public async Task<OperationResult<HttpStatusCode, bool>> RemoveProductAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var ProductDel = await Context.products.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (ProductDel == null)
                {
                    result.AddError("Product not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                ProductDel.IsValid = false;
                Context.products.Update(ProductDel);
                Context.SaveChangesAsync();
                result.EnumResult = HttpStatusCode.OK;
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.Result = false;
            }
            return result;
        }
        #endregion
    }
}
