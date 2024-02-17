using Emenu.Dto.Product;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepo;
        public ProductController(IProduct productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpGet]
        public IActionResult GetList(bool isDescending,int skip,int take,string? filter, string? sortingCol)
        {
            var result = _productRepo.GetAll(filter,sortingCol,isDescending,skip,take);
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] ProductDto dto)
        {
            var result = _productRepo.SetProductAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int productId)
        {
            var result = _productRepo.RemoveProductAsync(productId);
            return new JsonResult(result.Result);
        }
    }
}
