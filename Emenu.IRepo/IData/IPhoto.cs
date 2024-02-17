using Emenu.Dto.Photo;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface IPhoto
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<PhotoDto>>> GetAll();
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetPhotoAsync(PhotoDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemovePhotoAsync(int id);
        #endregion
    }
}
