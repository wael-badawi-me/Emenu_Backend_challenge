using Emenu.Dto.Color;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface IColor
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<ColorDto>>> GetAll();
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetColorAsync(ColorDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemoveColorAsync(int id);
        #endregion
    }
}
