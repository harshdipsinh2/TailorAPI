//using Microsoft.AspNetCore.Mvc;
//using TailorAPI.Models;
//using TailorAPI.Services;

//namespace TailorAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RoleController : ControllerBase
//    {
//        private readonly RoleService _roleService;

//        public RoleController(RoleService roleService)
//        {
//            _roleService = roleService;
//        }


//        [HttpGet]
//        public async Task<IActionResult> GetAllRoles()
//        {
//            var roles = await _roleService.GetAllRolesAsync();
//            return Ok(roles);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetRoleById(int id)
//        {
//            var role = await _roleService.GetRoleByIdAsync(id);
//            if (role == null) return NotFound("Role not found.");
//            return Ok(role);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateRole(int id, [FromBody] string newRoleName)
//        {
//            var result = await _roleService.UpdateRoleAsync(id, newRoleName);
//            if (!result) return NotFound("Role not found.");
//            return Ok("Role updated successfully.");
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteRole(int id)
//        {
//            var result = await _roleService.DeleteRoleAsync(id);
//            if (!result) return NotFound("Role not found.");
//            return Ok("Role deleted successfully.");
//        }
//    }
//}
