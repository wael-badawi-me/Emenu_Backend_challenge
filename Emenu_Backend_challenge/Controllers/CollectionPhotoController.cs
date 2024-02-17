using Emenu.Dto.CollectionPhoto;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectionPhotoController : ControllerBase
    {
        private readonly ICollectionPhoto _collectionPhotoRepo;
        public CollectionPhotoController(ICollectionPhoto collectionPhotoRepo)
        {
            _collectionPhotoRepo = collectionPhotoRepo;
        }


        [HttpGet]
        public IActionResult GetList(bool isDescending, int skip, int take, string? filter, string? sortingCol)
        {
            var result = _collectionPhotoRepo.GetAll(filter, sortingCol, isDescending, skip, take);
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] AddCollectionPhotoDto dto)
        {
            var result = _collectionPhotoRepo.SetCollectionPhotoAsync(dto);
            return new JsonResult(result.Result);
        }
    }
}
