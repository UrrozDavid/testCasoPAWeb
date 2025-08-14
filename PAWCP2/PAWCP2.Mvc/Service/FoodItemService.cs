using APW.Architecture;
using PAW.Architecture.Providers;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Entities;
using PAWCP2.Mvc.ViewModels;

namespace PAWCP2.Mvc.Service
{
    public interface IFoodItemService
    {
        Task<List<FoodItem>> GetByUserIdAsync(int userId);
        Task<List<FoodItemDto>> GetByRoleAsync(int roleId);
        Task<List<FoodItemDto>> FilterFoodItemsAsync(int roleId, FoodItemSearchViewModel filters);
        Task<List<string>> GetCategoriesAsync();
        Task<List<string>> GetBrandsAsync();
        Task<List<string>> GetSuppliersAsync();
    }

    public class FoodItemService : IFoodItemService
    {
        private readonly IRestProvider _restProvider;

        public FoodItemService(IRestProvider restProvider)
        {
            _restProvider = restProvider;
        }

        public async Task<List<FoodItem>> GetByUserIdAsync(int userId)
        {
            var response = await _restProvider.GetAsync("https://localhost:7099/UserRoles/", $"{userId}");
            var userRole = await JsonProvider.DeserializeAsync<UserRoleDto>(response);
            var roleId = userRole?.RoleId ?? 0;

            var responseFood = await _restProvider.GetAsync("https://localhost:7099/api/FoodItems/role/", $"{roleId}");
            var foodItemsDto = await JsonProvider.DeserializeAsync<List<FoodItemDto>>(responseFood);

            var foodItems = foodItemsDto.Select(dto => new FoodItem
            {
                FoodItemId = dto.FoodItemId,
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price
            }).ToList();

            return foodItems;
        }

        public async Task<List<FoodItemDto>> GetByRoleAsync(int roleId)
        {
            var response = await _restProvider.GetAsync("https://localhost:7099/api/FoodItems/role/", $"{roleId}");
            var foodItemsDto = await JsonProvider.DeserializeAsync<List<FoodItemDto>>(response);
            return foodItemsDto ?? new List<FoodItemDto>();
        }

        public async Task<List<FoodItemDto>> FilterFoodItemsAsync(int roleId, FoodItemSearchViewModel filters)
        {
            var items = await GetByRoleAsync(roleId);

            if (!string.IsNullOrEmpty(filters.Category))
                items = items.Where(x => x.Category == filters.Category).ToList();
            if (!string.IsNullOrEmpty(filters.Brand))
                items = items.Where(x => x.Brand == filters.Brand).ToList();
            if (filters.PriceMin.HasValue)
                items = items.Where(x => x.Price >= filters.PriceMin.Value).ToList();
            if (filters.PriceMax.HasValue)
                items = items.Where(x => x.Price <= filters.PriceMax.Value).ToList();
            if (filters.ExpirationDate.HasValue)
                items = items.Where(x => x.ExpirationDate >= filters.ExpirationDate.Value).ToList();
            if (filters.IsPerishable.HasValue)
                items = items.Where(x => x.IsPerishable == filters.IsPerishable.Value).ToList();
            if (filters.CaloriesMax.HasValue)
                items = items.Where(x => x.CaloriesPerServing <= filters.CaloriesMax.Value).ToList();
            if (!string.IsNullOrEmpty(filters.Supplier))
                items = items.Where(x => x.Supplier == filters.Supplier).ToList();
            if (filters.IsActive.HasValue)
                items = items.Where(x => x.IsActive == filters.IsActive.Value).ToList();

            return items;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var response = await _restProvider.GetAsync("https://localhost:7099/api/FoodItems/categories", null);
            var categories = await JsonProvider.DeserializeAsync<List<string>>(response);
            return categories ?? new List<string>();
        }

        public async Task<List<string>> GetBrandsAsync()
        {
            var response = await _restProvider.GetAsync("https://localhost:7099/api/FoodItems/brands", null);
            var brands = await JsonProvider.DeserializeAsync<List<string>>(response);
            return brands ?? new List<string>();
        }

        public async Task<List<string>> GetSuppliersAsync()
        {
            var response = await _restProvider.GetAsync("https://localhost:7099/api/FoodItems/suppliers", null);
            var suppliers = await JsonProvider.DeserializeAsync<List<string>>(response);
            return suppliers ?? new List<string>();
        }
    }
}
