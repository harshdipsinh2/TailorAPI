//using Microsoft.AspNetCore.Mvc;
//using TailorAPI.DTO.RequestDTO;
//using TailorAPI.Services;

//namespace TailorAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FabricTypeCombinedController : ControllerBase
//    {
//        private readonly FabricCombinedService _FabricCombinedService;

//        public FabricTypeCombinedController(FabricCombinedService fabricCombinedService)
//        {
//            _FabricCombinedService = fabricCombinedService;
//        }

//        // FabricType Endpoints
//        [HttpPost("AddFabricType")]
//        public IActionResult AddFabricType([FromBody] FabricTypeRequestDTO requestDTO)
//        {
//            var result = _fabricService.AddFabricType(requestDTO);
//            return Ok(result);
//        }

//        [HttpPut("UpdateFabricPrice")]
//        public IActionResult UpdateFabricPrice(int id, decimal newPrice)
//        {
//            var result = _fabricService.UpdateFabricPrice(id, newPrice);
//            return Ok(result);
//        }

//        [HttpGet("GetAllFabricTypes")]
//        public IActionResult GetAllFabricTypes()
//        {
//            var result = _fabricService.GetAllFabricTypes();
//            return Ok(result);
//        }

//        [HttpGet("GetFabricTypeById")]
//        public IActionResult GetFabricTypeById(int id)
//        {
//            var result = _fabricService.GetFabricTypeById(id);
//            return Ok(result);
//        }

//        [HttpDelete("SoftDeleteFabricType")]
//        public IActionResult SoftDeleteFabricType(int id)
//        {
//            var result = _fabricService.SoftDeleteFabricType(id);
//            return Ok(result);
//        }

//        // FabricStock Endpoints
//        [HttpPost("AddFabricStock")]
//        public IActionResult AddFabricStock([FromBody] FabricStockRequestDTO requestDTO)
//        {
//            var result = _fabricService.AddFabricStock(requestDTO);
//            return Ok(result);
//        }

//        [HttpGet("GetAllFabricStocks")]
//        public IActionResult GetAllFabricStocks()
//        {
//            var result = _fabricService.GetAllFabricStocks();
//            return Ok(result);
//        }

//        [HttpGet("GetFabricStockById")]
//        public IActionResult GetFabricStockById(int id)
//        {
//            var result = _fabricService.GetFabricStockById(id);
//            return Ok(result);
//        }
//    }
//}
