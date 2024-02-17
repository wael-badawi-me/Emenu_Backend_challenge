using Emenu.Dto.Collection;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollection _collectionRepo;
        public CollectionController(ICollection collectionRepo)
        {
            _collectionRepo = collectionRepo;
        }


        [HttpGet]
        public IActionResult GetList(bool isDescending, int skip, int take, string? filter, string? sortingCol)
        {
            var result = _collectionRepo.GetAll(filter, sortingCol, isDescending, skip, take);
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] AddCollectionDto dto)
        {
            var result = _collectionRepo.SetCollectionAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int storeId)
        {
            var result = _collectionRepo.RemoveCollectionAsync(storeId);
            return new JsonResult(result.Result);
        }
    }
}
