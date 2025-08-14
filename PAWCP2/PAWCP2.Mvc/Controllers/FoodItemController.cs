using Microsoft.AspNetCore.Mvc;
using PAWCP2.Mvc.Service;
using PAWCP2.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PAWCP2.Mvc.ViewModels;

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
        public async Task<IActionResult> Index(FoodItemSearchViewModel filters, bool clear = false)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
                return Unauthorized();

            int? userRoleId = await _userRolesService.GetRoleIdByUserId(userId);
            if (!userRoleId.HasValue)
                return Forbid();

            if (clear)
            {
                filters.FoodItems = await _service.GetByRoleAsync(userRoleId.Value);
            }
            else
            {
                filters.FoodItems = await _service.FilterFoodItemsAsync(userRoleId.Value, filters);
            }

            filters.Categories = await _service.GetCategoriesAsync();
            filters.Brands = await _service.GetBrandsAsync();
            filters.Suppliers = await _service.GetSuppliersAsync();

            return View(filters);
        }
    }
}
