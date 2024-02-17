using Emenu.Dto.Color;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColor _colorRepo;
        public ColorController(IColor colorRepo)
        {
            _colorRepo = colorRepo;
        }


        [HttpGet]
        public IActionResult GetList() 
        {
            var result=_colorRepo.GetAll();
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] ColorDto dto)
        {
            var result = _colorRepo.SetColorAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int colorId)
        {
            var result = _colorRepo.RemoveColorAsync(colorId);
            return new JsonResult(result.Result);
        }
    }
}
