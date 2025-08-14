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
            public async Task<IActionResult> GetByRole(int roleId)
            {
                try
                {
                    var items = await _manager.GetByRoleAsync(roleId);
                var dtos = items.Select(item => new FoodItemDto
                {
                    FoodItemId = item.FoodItemId,
                    Name = item.Name,
                    Category = item.Category,
                    Price = item.Price,
                    Brand = item.Brand,
                    IsPerishable = item.IsPerishable,
                    CaloriesPerServing = item.CaloriesPerServing,
                    Supplier = item.Supplier,
                    IsActive = item.IsActive,
                    PriceRange = item.Price > 0 ? item.Price : 0,
                    ExpirationDate = item.ExpirationDate
                }).ToList();

                return Ok(dtos);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

        // GET: api/fooditems/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _manager.GetCategoriesAsync();
            return Ok(categories);
        }
    
        // GET: api/fooditems/brands
        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                var brands = await _manager.GetBrandsAsync();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/fooditems/suppliers
        [HttpGet("suppliers")]
        public async Task<IActionResult> GetSuppliers()
        {
            try
            {
                var suppliers = await _manager.GetSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

