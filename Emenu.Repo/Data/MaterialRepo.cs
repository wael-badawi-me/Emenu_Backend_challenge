using Emenu.Dto.Material;


namespace Emenu.Repo.Data
{
    public class MaterialRepo: EmenuRepository, IMaterial
    {
        public MaterialRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<MaterialDto>>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, List<MaterialDto>>();
            var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<MaterialEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("Material not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }

                var Materials = Context.materials.Where(e => e.IsValid)
                 .Select(e => new MaterialDto()
                 {
                     id = e.Id,
                     name = e.Name,

                 }).ToList();

                result.Result = Materials;
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
        public async Task<OperationResult<HttpStatusCode, bool>> SetMaterialAsync(MaterialDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {

                #region Validation 
                if (string.IsNullOrEmpty(dto.name))
                {
                    result.AddError("please insert Material name");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion
                bool isAdd = dto.id == 0;
                MaterialEntity Material = new MaterialEntity();
                Material.Name = dto.name;
                if (isAdd)
                    await Context.materials.AddAsync(Material);
                else
                {
                    Material.Id = dto.id;
                    Context.materials.Update(Material);

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
        public async Task<OperationResult<HttpStatusCode, bool>> RemoveMaterialAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var MaterialDel = await Context.materials.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (MaterialDel == null)
                {
                    result.AddError("Material not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                MaterialDel.IsValid = false;
                Context.materials.Update(MaterialDel);
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
