using Emenu.Dto.Photo;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhoto _photoRepo;
        public PhotoController(IPhoto photoRepo)
        {
            _photoRepo = photoRepo;
        }


        [HttpGet]
        public IActionResult GetList()
        {
            var result = _photoRepo.GetAll();
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] PhotoDto dto)
        {
            var result = _photoRepo.SetPhotoAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int productId)
        {
            var result = _photoRepo.RemovePhotoAsync(productId);
            return new JsonResult(result.Result);
        }
    }
}
