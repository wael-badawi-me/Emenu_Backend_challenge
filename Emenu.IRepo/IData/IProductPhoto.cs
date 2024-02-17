using Emenu.Dto.ProductPhoto;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface IProductPhoto
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<ProductPhotoDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take);
        #endregion

        #region Set
         Task<OperationResult<HttpStatusCode, bool>> SetProductPhotoAsync(AddProductPhotoDto dto);
        #endregion

   
    }
}
