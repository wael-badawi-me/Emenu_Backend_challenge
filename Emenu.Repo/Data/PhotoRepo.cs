using Emenu.Dto.Photo;

namespace Emenu.Repo.Data
{
    public class PhotoRepo: EmenuRepository,IPhoto
    {
        public PhotoRepo(DataContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        #region Get
        public async Task<OperationResult<HttpStatusCode, List<PhotoDto>>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, List<PhotoDto>>();
            using var trans = await Context.Database.BeginTransactionAsync();
            try
            {
                var exsist = await CheckEntityExsist<PhotoEntity>(e => e.IsValid);
                if (!exsist)
                {
                    result.AddError("Photo not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    return result;
                }

                var Photos = Context.photos.Where(e => e.IsValid)
                 .Select(e => new PhotoDto()
                 {
                     id = e.Id,
                     url=e.URL

                 }).ToList();

                result.Result = Photos;
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
        public async Task<OperationResult<HttpStatusCode, bool>> SetPhotoAsync(PhotoDto dto)
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
                {
                    Photo.Id=dto.id;
                    Context.photos.Update(Photo);
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
        public async Task<OperationResult<HttpStatusCode, bool>> RemovePhotoAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            try
            {
                var PhotoDel = await Context.photos.FirstOrDefaultAsync(e => e.IsValid && e.Id == id);
                if (PhotoDel == null)
                {
                    result.AddError("Photo not found");
                    result.EnumResult = HttpStatusCode.NotFound;
                    result.Result = false;
                    return result;
                }
                PhotoDel.IsValid = false;
                Context.photos.Update(PhotoDel);
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
