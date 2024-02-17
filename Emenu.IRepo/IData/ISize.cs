using Emenu.Dto.Size;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface ISize
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<SizeDto>>> GetAll();
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetSizeAsync(SizeDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemoveSizeAsync(int id);
        #endregion
    }
}
