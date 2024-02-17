using Emenu.Dto.Photo;
using Emenu.Dto.Product;
using Emenu.Dto.Collection;
using Emenu.Dto.CollectionPhoto;
using System.Linq;

namespace Emenu.Repo.Data
{
    public class StorePhotoRepo: EmenuRepository, ICollectionPhoto
    {

        public StorePhotoRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<CollectionPhotoDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take)
        {
            var result = new OperationResult<HttpStatusCode, List<CollectionPhotoDto>>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<StorePhotoEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("no photos found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }
                //start build quary
                var quary = Context.storePhotos.Where(e => e.IsValid).GroupBy(e => e.StoreId);

                //filter 
                if (!string.IsNullOrEmpty(filter))
                {
                    quary = quary.Where(e => e.Any(s => s.Store.Product.NameEn.Contains(filter)) || e.Any(s => s.Store.Product.NameAr.Contains(filter)) || e.Any(s => s.Store.Product.Description.Contains(filter))
                    || e.Any(s => s.Store.Size.Name.Contains(filter)) || e.Any(s => s.Store.Color.Name.Contains(filter))|| e.Any(s => s.Store.Material.Name.Contains(filter)));
                }
                //applaying skip,take
                result.Result = quary.Skip(skip).Take(take)
               .Select(group => new CollectionPhotoDto()
               {
                   CollectionId = group.Key,
                   Collection = group.Select(c => new MainCollectionDto()
                   {

                       product = Context.products.Where(p => p.IsValid && p.Id == c.Store.ProductId).Select(w => new MainProductDto()
                       {
                           id = w.Id,
                           nameAr = w.NameAr,
                           nameEn = w.NameEn,
                           description = w.Description,
                           mainPhotoUrl = w.MainPhoto.URL,
                           color = c.Store.Color.Name,
                           material = c.Store.Material.Name,
                           size = c.Store.Size.Name,
                           photoes = Context.storePhotos.Where(e => e.IsValid && e.StoreId == c.StoreId).Select(phto => new PhotoDto() { id = phto.Photo.Id, url = phto.Photo.URL }).ToList(),
                       }).First()


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
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.nameEn).ToList();
                                break;
                            case "namear":
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.nameAr).ToList();
                                break;
                            case "description":
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.description).ToList();
                                break;
                            case "colorname":
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.color).ToList();
                                break;
                            case "sizename":
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.size).ToList();
                                break;
                            case "materialname":
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.material).ToList();
                                break;
                            default:
                                result.Result = result.Result.OrderByDescending(e => e.Collection.product.id).ToList();
                                break;
                        }
                    }
                    else
                    {
                        result.Result = result.Result.OrderByDescending(e => e.Collection.product.id).ToList();
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "nameen":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.nameEn).ToList();
                                break;
                            case "namear":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.nameAr).ToList();
                                break;
                            case "description":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.description).ToList();
                                break;
                            case "colorname":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.color).ToList();
                                break;
                            case "sizename":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.size).ToList();
                                break;
                            case "materialname":
                                result.Result = result.Result.OrderBy(e => e.Collection.product.material).ToList();
                                break;
                            default:
                                result.Result = result.Result.OrderBy(e => e.Collection.product.id).ToList();
                                break;
                        }
                    }
                    else
                    {
                        result.Result = result.Result.OrderBy(e => e.Collection.product.id).ToList();
                    }

                }

                result.EnumResult = HttpStatusCode.OK;
                await trans.CommitAsync();



                //var Collections = Context.storePhotos.Where(e => e.IsValid).GroupBy(e => e.StoreId).Select(group => new CollectionPhotoDto()
                //{
                //    CollectionId = group.Key,
                //    Collection = group.Select(c => new MainCollectionDto()
                //    {
                        
                //        product = Context.products.Where(p => p.IsValid && p.Id == c.Store.ProductId).Select(w => new MainProductDto()
                //        {
                //            id = w.Id,
                //            nameAr = w.NameAr,
                //            nameEn = w.NameEn,
                //            description = w.Description,
                //            mainPhotoUrl = w.MainPhoto.URL,
                //            color = c.Store.Color.Name,
                //            material = c.Store.Material.Name,
                //            size = c.Store.Size.Name,
                //            photoes =Context.storePhotos.Where(e=>e.IsValid && e.StoreId==c.StoreId).Select(phto => new PhotoDto() { id = phto.Photo.Id, url = phto.Photo.URL }).ToList(),
                //        }).First()


                //    }).First()
                //}).ToList();


                //result.Result = Collections;
                //result.EnumResult = HttpStatusCode.OK;
                //await trans.CommitAsync();
            }
            catch (Exception e)
            {
                await trans.RollbackAsync();
                result.AddError("error");
                result.EnumResult = HttpStatusCode.InternalServerError;
            }
            return result;
        }
        #endregion

        #region Set
        public async Task<OperationResult<HttpStatusCode, bool>> SetCollectionPhotoAsync(AddCollectionPhotoDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                if (dto.collectionId == 0 ||dto.photos == null)
                {
                    await trans.RollbackAsync();
                    result.AddError("Some Fields are empty");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                bool isAdd = dto.id == 0;
                if (isAdd)
                {
                    var photoSet = await Context.storePhotos.Where(we => we.IsValid && we.StoreId == dto.collectionId).ToListAsync();
                    foreach (var item in photoSet)
                    {
                        item.IsValid = false;
                    }
                    Context.storePhotos.UpdateRange(photoSet);
                    Context.SaveChanges();
                }
               
                List<StorePhotoEntity> storePhotoEntities = new List<StorePhotoEntity>();

                   
           
                if (isAdd && (dto.photos == null || dto.photos.Count == 0))
                {
                    await trans.RollbackAsync();
                    result.AddError("Please select one photo at least");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                bool exsist;

                foreach (var item in dto.photos)
                {
                    exsist = await CheckEntityExsist<PhotoEntity>(e => e.IsValid && e.URL == item);
                    if (!exsist)
                    {
                        await Context.photos.AddAsync(new PhotoEntity() { URL = item });
                        Context.SaveChanges();
                    }
                    storePhotoEntities.Add(new StorePhotoEntity()
                    {
                        StoreId=dto.collectionId,
                        PhotoId= Context.photos.First(e => e.IsValid && e.URL == item).Id,
                });
                }
                if (isAdd)
                    await Context.storePhotos.AddRangeAsync(storePhotoEntities);
                else 
                {
                    storePhotoEntities.ForEach(item =>item.StoreId = dto.id);
                    Context.storePhotos.UpdateRange(storePhotoEntities);
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
