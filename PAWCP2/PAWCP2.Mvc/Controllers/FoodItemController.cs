using Microsoft.AspNetCore.Mvc;
using PAWCP2.Mvc.Service;
using PAWCP2.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PAWCP2.Mvc.Controllers
{
    public class FoodItemController : Controller
    {
        private readonly IFoodItemService _service;
        private readonly IUserRolesService _userRolesService;

        public FoodItemController(IFoodItemService service, IUserRolesService userRolesService)
        {
            _service = service;
            _userRolesService = userRolesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
                return Unauthorized();
            var foodItems = await _service.GetByUserIdAsync(userId);
            return View(foodItems);
        }
    }
}
