using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{

    public class BranchController : ControllerBase
    {
        public readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("add-branch")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchRequestDTO dto)
        {
            var result = await _branchService.CreateBranchAsync(dto);
            if (result == null)
                return BadRequest("Invalid ShopId or failed to create branch.");

            return Ok(result);
        }
    }

}

