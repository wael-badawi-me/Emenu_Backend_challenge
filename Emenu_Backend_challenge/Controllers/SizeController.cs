using Emenu.Dto.Size;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISize _sizeRepo;
        public SizeController(ISize sizeRepo)
        {
            _sizeRepo = sizeRepo;
        }


        [HttpGet]
        public IActionResult GetList()
        {
            var result = _sizeRepo.GetAll();
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] SizeDto dto)
        {
            var result = _sizeRepo.SetSizeAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int sizeId)
        {
            var result = _sizeRepo.RemoveSizeAsync(sizeId);
            return new JsonResult(result.Result);
        }
    }
}
