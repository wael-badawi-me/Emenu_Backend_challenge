

using Emenu.Dto.Color;


namespace Emenu.Repo.Data
{
    public class ColorRepo:EmenuRepository, IColor
    {
        public ColorRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<ColorDto>>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, List<ColorDto>>();
            var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<ColorEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("Color not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }

                var colors = Context.colors.Where(e => e.IsValid)
                 .Select(e => new ColorDto()
                 {
                     id = e.Id,
                     name = e.Name,

                 }).ToList();

                result.Result = colors;
                result.EnumResult = HttpStatusCode.OK;
                await trans.CommitAsync();
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
        public async Task<OperationResult<HttpStatusCode, bool>> SetColorAsync(ColorDto dto)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {

                #region Validation 
                if (string.IsNullOrEmpty(dto.name))
                {
                    result.AddError("please insert Color name");
                    result.EnumResult = HttpStatusCode.BadRequest;
                    return result;
                }
                #endregion
                bool isAdd = dto.id == 0;
                ColorEntity color = new ColorEntity();
                color.Name = dto.name;
                if (isAdd)
                    await Context.colors.AddAsync(color);
                else
                {
                    color.Id = dto.id;
                    Context.colors.Update(color);
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
        public async Task<OperationResult<HttpStatusCode, bool>> RemoveColorAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var ColorDel = await Context.colors.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (ColorDel == null)
                {
                    result.AddError("color not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                ColorDel.IsValid = false;
                Context.colors.Update(ColorDel);
                Context.SaveChangesAsync();
                result.EnumResult= HttpStatusCode.OK;
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
