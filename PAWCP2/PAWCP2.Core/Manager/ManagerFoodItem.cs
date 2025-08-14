using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Core.Repositories;
using PAWCP2.Models.Entities;

namespace PAWCP2.Core.Manager
{
    public interface IManagerFoodItem
    {
        Task<IEnumerable<FoodItem>> GetByRoleAsync(int roleId);
        Task<List<string>> GetCategoriesAsync();
        Task<List<string>> GetBrandsAsync();
        Task<List<string>> GetSuppliersAsync();
    }

    public class ManagerFoodItem : IManagerFoodItem
    {
        private readonly IRepositoryFoodItem _repository;

        public ManagerFoodItem(IRepositoryFoodItem repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FoodItem>> GetByRoleAsync(int roleId)
        {
            return await _repository.ReadByRoleAsync(roleId);
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _repository.GetCategoriesAsync();
        }
        public async Task<List<string>> GetBrandsAsync()
        => await _repository.GetBrandsAsync();

        public async Task<List<string>> GetSuppliersAsync()
            => await _repository.GetSuppliersAsync();
    }
}
