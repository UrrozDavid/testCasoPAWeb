using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Core.Manager;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Entities;

namespace PAWCP2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly IManagerFoodItem _manager;

        public FoodItemsController(IManagerFoodItem manager)
        {
            _manager = manager;
        }

        // GET: api/fooditems/role/2
        [HttpGet("role/{roleId}")]
        public async Task<IEnumerable<FoodItemDto>> GetByRole(int roleId)
        {
            var items = await _manager.GetByRoleAsync(roleId);
            return items.Select(item => new FoodItemDto
            {
                FoodItemId = item.FoodItemId,
                Name = item.Name,
                Category = item.Category,
                Price = item.Price
            });
        }

    }
}
