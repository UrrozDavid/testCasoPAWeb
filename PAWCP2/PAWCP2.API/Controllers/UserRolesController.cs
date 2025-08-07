using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Entities;

namespace PAWCP2.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRolesController (IManagerUserRole managerUserRole) : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<UserRoleDto>> Get() => await managerUserRole.ReadDtoAsync();

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserRoleDto>> GetById(int userId)
            => (await managerUserRole.ReadDtoAsync()).FirstOrDefault(x => x.UserId == userId) is { } x ? Ok(x) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] IEnumerable<UserRoleCreateDTO> items) 
        {
            if (items is null || !items.Any()) return BadRequest("Empty");

            foreach (var item in items)
            {
                var entity = new UserRole
                {
                    UserId = item.UserId,
                    RoleId = item.RoleId,
                    Description = item.Description
                };

                var result= await managerUserRole.SaveUserRoleAsync(entity);
                if (!result) return BadRequest($"Could not save role for user {item.UserId}");
            }

            return Ok(true); 
        }

        [HttpDelete("{userId:int}/{roleId:int}")]
        public async Task<IActionResult> Delete(int userId, int roleId)
            => Ok(await managerUserRole.DeleteUserRoleAsync(new UserRole { UserId = userId, RoleId = roleId }));
    }
}
