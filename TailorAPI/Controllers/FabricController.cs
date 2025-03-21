using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTOs.Request;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/fabric")]
    [ApiController]
    public class FabricController : ControllerBase
    {
        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService)
        {
            _fabricService = fabricService;
        }

        [HttpPost("add-fabric")]
        public async Task<IActionResult> AddFabric([FromBody] FabricRequestDTO fabricDto)
        {
            var result = await _fabricService.AddFabric(fabricDto);
            return Ok(result);
        }

        [HttpGet("get-fabrics")]
        public async Task<IActionResult> GetAllFabrics()
        {
            var fabrics = await _fabricService.GetAllFabrics();
            return Ok(fabrics);
        }

        [HttpDelete("delete")]  // ID via Query Parameter
        public async Task<IActionResult> DeleteFabric([FromQuery] int fabricId)
        {
            var result = await _fabricService.DeleteFabric(fabricId);
            if (!result) return NotFound("Fabric not found or already deleted.");
            return Ok("Fabric deleted successfully.");
        }
        [HttpGet("get-fabric/{fabricId}")]
        public async Task<IActionResult> GetFabricById(int fabricId)
        {
            var fabric = await _fabricService.GetFabricById(fabricId);
            if (fabric == null) return NotFound("Fabric not found.");
            return Ok(fabric);
        }

    }
}