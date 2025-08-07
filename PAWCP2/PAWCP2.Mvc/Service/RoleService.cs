using PAWCP2.Core.Models;
using PAWCP2.Core.Manager;
using PAWCP2.Models.Entities;

namespace PAWCP2.Mvc.Service
{
    public interface IRoleService
    {
        // CRUD
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Role user);
        Task<bool> UpdateAsync(Role user);
        Task<bool> DeleteAsync(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly IManagerRole _business;

        public RoleService(IManagerRole business)
        {
            _business = business;
        }

        // ----------------- CRUD -----------------
        public async Task<List<Role>> GetAllAsync() => (await _business.GetAllRolesAsync()).ToList();
        public async Task<Role?> GetByIdAsync(int id) => await _business.GetRoleAsync(id);

        public async Task<bool> CreateAsync(Role role)
            => await _business.SaveRoleAsync(role);

        public async Task<bool> UpdateAsync(Role role)
            => await _business.SaveRoleAsync(role);

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _business.GetRoleAsync(id);
            if (role == null) return false;
            return await _business.DeleteRoleAsync(role);
        }
    }
}