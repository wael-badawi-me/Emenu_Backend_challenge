using Emenu.Dto.Size;

namespace Emenu.Repo.Data
{
    public class SizeRepo: EmenuRepository, ISize
    {
        public SizeRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<SizeDto>>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, List<SizeDto>>();
            var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<SizeEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("Size not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }

                var sizes = Context.sizes.Where(e => e.IsValid)
                 .Select(e => new SizeDto()
                 {
                     id = e.Id,
                     name = e.Name,

                 }).ToList();

                result.Result = sizes;
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
        public async Task<OperationResult<HttpStatusCode, bool>> SetSizeAsync(SizeDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {

                #region Validation 
                if (string.IsNullOrEmpty(dto.name))
                {
                    result.AddError("please insert Size name");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion
                bool isAdd = dto.id == 0;
                SizeEntity size = new SizeEntity();
                size.Name = dto.name;
                if (isAdd)
                    await Context.sizes.AddAsync(size);
                else {
                    size.Id = dto.id;
                    Context.sizes.Update(size);
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
        public async Task<OperationResult<HttpStatusCode, bool>> RemoveSizeAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var SizeDel = await Context.sizes.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (SizeDel == null)
                {
                    result.AddError("size not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                SizeDel.IsValid = false;
                Context.sizes.Update(SizeDel);
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
