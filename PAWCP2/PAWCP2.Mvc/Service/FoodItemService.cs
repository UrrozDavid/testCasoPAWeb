using APW.Architecture;
using PAW.Architecture.Providers;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Entities;

namespace PAWCP2.Mvc.Service
{
    public interface IFoodItemService
    {
        Task<List<FoodItem>> GetByUserIdAsync(int userId);
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




    }
}
