using Emenu.Dto.Collection;

namespace Emenu.Repo.Data
{
    public class StoreRepo: EmenuRepository, ICollection
    {
        public StoreRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<CollectionDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take)
        {
            var result = new OperationResult<HttpStatusCode, List<CollectionDto>>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<StoreEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("No Product found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }
                // build quary
                var quary = Context.stores.Where(e => e.IsValid);
                // applaying filter
                if (!string.IsNullOrEmpty(filter))
                {
                    quary = quary.Where(e => e.Product.NameEn.Contains(filter) || e.Product.NameAr.Contains(filter) || e.Product.Description.Contains(filter) || e.Color.Name.Contains(filter) || e.Material.Name.Contains(filter) || e.Size.Name.Contains(filter));
                }
                // applaying sort
                if (isDesending)
                {
                    if (!string.IsNullOrEmpty(sortingCol))
                    {
                        switch (sortingCol.ToLower())
                        {
                            case "productnameen":
                                quary = quary.OrderByDescending(e => e.Product.NameEn);
                                break;
                            case "productnamear":
                                quary = quary.OrderByDescending(e => e.Product.NameAr);
                                break;
                            case "productdescription":
                                quary = quary.OrderByDescending(e => e.Product.Description);
                                break;
                            case "colorname":
                                quary = quary.OrderByDescending(e => e.Color.Name);
                                break;
                            case "sizename":
                                quary = quary.OrderByDescending(e => e.Size.Name);
                                break;
                            case "materialname":
                                quary = quary.OrderByDescending(e => e.Material.Name);
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
                            case "productnameen":
                                quary = quary.OrderBy(e => e.Product.NameEn);
                                break;
                            case "productnamear":
                                quary = quary.OrderBy(e => e.Product.NameAr);
                                break;
                            case "productdescription":
                                quary = quary.OrderBy(e => e.Product.Description);
                                break;
                            case "colorname":
                                quary = quary.OrderBy(e => e.Color.Name);
                                break;
                            case "sizename":
                                quary = quary.OrderBy(e => e.Size.Name);
                                break;
                            case "materialname":
                                quary = quary.OrderBy(e => e.Material.Name);
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
                result.Result = quary.Skip(skip).Take(take).Select(e => new CollectionDto()
                {
                    id = e.Id,
                    productDescription = e.Product.Description,
                    productMainPhotoUrl = e.Product.MainPhoto.URL,
                    productNameAr = e.Product.NameAr,
                    productNameEn = e.Product.NameEn,
                    colorName = e.Color.Name,
                    materialName = e.Material.Name,
                    sizeName = e.Size.Name,
                    productId = e.ProductId,
                    colorId = e.Color.Id,   
                    materialId = e.Material.Id,
                    sizeId=e.SizeId

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
        public async Task<OperationResult<HttpStatusCode, bool>> SetCollectionAsync(AddCollectionDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                #region Validation 
                if (dto.productId==0)
                {
                    result.AddError("please Choose Product");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                if (dto.sizeId == 0 && dto.colorId == 0 && dto.materialId == 0)
                {
                    result.AddError("please Choose Size or Color or Material");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
             
                #endregion
                bool isAdd = dto.id == 0;
                StoreEntity Store = new StoreEntity();
                Store.ColorId = dto.colorId;
                Store.MaterialId = dto.materialId;
                Store.SizeId=dto.sizeId;
                Store.ProductId=dto.productId;
                if (isAdd)
                    await Context.stores.AddAsync(Store);
                else 
                {
                    Store.Id = dto.id;
                    Context.stores.Update(Store);
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
        #endregion

        #region Delete
        public async Task<OperationResult<HttpStatusCode, bool>> RemoveCollectionAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var StoreDel = await Context.stores.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (StoreDel == null)
                {
                    result.AddError("no Product found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                StoreDel.IsValid = false;
                Context.stores.Update(StoreDel);
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
