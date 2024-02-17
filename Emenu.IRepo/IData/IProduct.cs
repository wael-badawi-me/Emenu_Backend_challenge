using Emenu.Dto.Product;
using Shared.OperationResults;
using System.Net;

namespace Emenu.IRepo.IData
{
    public interface IProduct
    {
        #region Get
        Task<OperationResult<HttpStatusCode, List<ProductDto>>> GetAll(string filter, string sortingCol, bool isDesending, int skip, int take);
        #endregion

        #region Set
        Task<OperationResult<HttpStatusCode, bool>> SetProductAsync(ProductDto dto);
        #endregion

        #region Delete
        Task<OperationResult<HttpStatusCode, bool>> RemoveProductAsync(int id);
        #endregion
    }
}
