using Emenu.Dto.CollectionPhoto;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface ICollectionPhoto
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<CollectionPhotoDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take);
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetCollectionPhotoAsync(AddCollectionPhotoDto dto);
        #endregion


    }
}
