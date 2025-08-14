using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Models;
using PAWCP2.Models.Entities;
using TBA.Repositories;

namespace PAWCP2.Core.Repositories
{
    public interface IRepositoryFoodItem
    {
        Task<IEnumerable<FoodItem>> ReadAsync();
        Task<IEnumerable<FoodItem>> ReadByRoleAsync(int roleId);
        Task<List<string>> GetCategoriesAsync();
        Task<List<string>> GetBrandsAsync();
        Task<List<string>> GetSuppliersAsync();
    }

    public class RepositoryFoodItem : RepositoryBase<FoodItem>, IRepositoryFoodItem
    {
        public RepositoryFoodItem(FoodbankContext context) : base(context) { }

        public async Task<IEnumerable<FoodItem>> ReadAsync()
            => await _context.FoodItems.ToListAsync();

        public async Task<IEnumerable<FoodItem>> ReadByRoleAsync(int roleId)
        {
            return await _context.FoodItems
                .Where(f => f.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Category != null)
                .Select(f => f.Category!)
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<string>> GetBrandsAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Brand != null)
                .Select(f => f.Brand!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetSuppliersAsync()
        {
            return await _context.FoodItems
                .Where(f => f.Supplier != null)
                .Select(f => f.Supplier!)
                .Distinct()
                .ToListAsync();
        }

    }
}
