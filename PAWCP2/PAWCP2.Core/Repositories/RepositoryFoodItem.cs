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

    }
}
