using Emenu.Dto.ProductPhoto;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IProductPhoto _productPhotoRepo;
        public ProductPhotoController(IProductPhoto productPhotoRepo)
        {
            _productPhotoRepo = productPhotoRepo;
        }


        [HttpGet]
        public IActionResult GetList(bool isDescending, int skip, int take, string? filter, string? sortingCol)
        {
            var result = _productPhotoRepo.GetAll(filter, sortingCol, isDescending, skip, take);
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] AddProductPhotoDto dto)
        {
            var result = _productPhotoRepo.SetProductPhotoAsync(dto);
            return new JsonResult(result.Result);
        }
 
    }
}
