//using Microsoft.AspNetCore.Mvc;
//using TailorAPI.DTO.RequestDTO;
//using TailorAPI.Services;
//using TailorAPI.Services.Interface;
//using System.Text.Json.Serialization;
//using System.Runtime.CompilerServices; // Add this namespace


//namespace TailorAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FabricTypeCombinedController : ControllerBase
//    {
//        private readonly IFabricCombinedService _fabricCombinedService;

//        public FabricTypeCombinedController(IFabricCombinedService fabricCombinedService)
//        {
//            _fabricCombinedService = fabricCombinedService;
//        }


//        // FabricType Endpoints
//        [HttpPost("AddFabricType")]
//        public async Task<IActionResult> AddFabricType([FromBody] FabricTypeRequestDTO requestDTO)
//        {
//            var result = await _fabricCombinedService.AddFabricTypeAsync(requestDTO);
//            return Ok(result);
//        }

//        [HttpPut("UpdateFabricPrice")]
//        public async Task<IActionResult> UpdateFabricPrice(int id, decimal newPrice)
//        {
//            var result = await _fabricCombinedService.UpdateFabricTypePriceAsync(id, newPrice);
//            return Ok(result);
//        }

//        [HttpGet("GetAllFabricTypes")]
//        public async Task<IActionResult> GetAllFabricTypes()
//        {
//            var result = await _fabricCombinedService.GetAllFabricTypesAsync();
//            return Ok(result);
//        }

//        [HttpGet("GetFabricTypeById")]
//        public async Task<IActionResult> GetFabricTypeById(int id)
//        {
//            var result = await _fabricCombinedService.GetFabricTypeByIdAsync(id);
//            return Ok(result);
//        }

//        [HttpDelete("SoftDeleteFabricType")]
//        public async Task<IActionResult> SoftDeleteFabricType(int id)
//        {
//            var result = await _fabricCombinedService.SoftDeleteFabricTypeAsync(id);
//            return Ok(result);
//        }


//        // FabricStock Endpoints
//        [HttpPost("AddFabricStock")]
//        public async Task<IActionResult> AddFabricStock([FromBody] FabricStockRequestDTO requestDTO)
//        {
//            var result = await _fabricCombinedService.AddFabricStockAsync(requestDTO);
//            return Ok(result);
//        }

//        [HttpGet("GetAllFabricStocks")]
//        public async Task<IActionResult> GetAllFabricStocks()
//        {
//            var result = await _fabricCombinedService.GetAllFabricStocksAsync();
//            return Ok(result);
//        }

//        [HttpGet("GetFabricStockById")]
//        public async Task<IActionResult> GetFabricStockById(int id)
//        {
//            var result = await _fabricCombinedService.GetFabricStockByIdAsync(id);
//            return Ok(result);
//        }
//    }
//}
