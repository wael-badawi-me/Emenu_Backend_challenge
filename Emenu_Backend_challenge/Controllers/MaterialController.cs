using Emenu.Dto.Material;
using Emenu.IRepo.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emenu_Backend_challenge.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterial _materialRepo;
        public MaterialController(IMaterial materialRepo)
        {
            _materialRepo = materialRepo;
        }


        [HttpGet]
        public IActionResult GetList()
        {
            var result = _materialRepo.GetAll();
            return new JsonResult(result.Result);
        }
        [HttpPost]
        public IActionResult Save([FromBody] MaterialDto dto)
        {
            var result = _materialRepo.SetMaterialAsync(dto);
            return new JsonResult(result.Result);
        }
        [HttpDelete]
        public IActionResult Delete(int materialId)
        {
            var result = _materialRepo.RemoveMaterialAsync(materialId);
            return new JsonResult(result.Result);
        }
    }
}
