
using Emenu.Dto.Material;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface IMaterial
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<MaterialDto>>> GetAll();
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetMaterialAsync(MaterialDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemoveMaterialAsync(int id);
        #endregion
    }
}
