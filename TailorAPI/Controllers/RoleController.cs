namespace TailorAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using TailorAPI.Services.Interface;
    using TailorAPI.DTO;

    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDto)
        {
            if (string.IsNullOrEmpty(roleDto.RoleName))
                return BadRequest("Role name is required.");

            var result = await _roleService.CreateRoleAsync(roleDto.RoleName);
            if (!result)
                return BadRequest("Role already exists or could not be created.");

            return Ok(new { message = "Role created successfully." });
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<IdentityRole>>> GetAllRoles()
        {
            return Ok(await _roleService.GetAllRolesAsync());
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult<IdentityRole>> GetRoleById(string roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
                return NotFound("Role not found.");

            return Ok(role);
        }

        [HttpPut("update/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] string newRoleName)
        {
            var result = await _roleService.UpdateRoleAsync(roleId, newRoleName);
            if (!result)
                return BadRequest("Role update failed.");

            return Ok("Role updated successfully.");
        }

        [HttpDelete("delete/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result)
                return BadRequest("Role deletion failed.");

            return Ok("Role deleted successfully.");
        }
    }

}
