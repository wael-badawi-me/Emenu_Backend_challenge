using Emenu.Dto.Collection;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface ICollection
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<CollectionDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take);
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetCollectionAsync(AddCollectionDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemoveCollectionAsync(int id);
        #endregion
    }
}
