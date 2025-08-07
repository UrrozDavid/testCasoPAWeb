using PAWCP2.Core.Models;
using PAWCP2.Core.Manager;

namespace PAWCP2.Mvc.Service
{
    public interface IUserRolesService
    {
        // CRUD
        Task<List<UserRole>> GetAllAsync();
        Task<UserRole?> GetByIdAsync(int id);
        Task<bool> CreateAsync(UserRole userRole);
        Task<bool> UpdateAsync(UserRole userRole);
        Task<bool> DeleteAsync(int id);
    }

    public class UserRolesService : IUserRolesService
    {
        private readonly ManagerUserRole _business;

        public UserRolesService(ManagerUserRole business)
        {
            _business = business;
        }

        // ----------------- CRUD -----------------
        public async Task<List<UserRole>> GetAllAsync() => (await _business.GetAllUserRolesAsync()).ToList();
        public async Task<UserRole?> GetByIdAsync(int id) => await _business.GetUserRoleAsync(id);

        public async Task<bool> CreateAsync(UserRole userRole)
            => await _business.SaveUserRoleAsync(userRole);

        public async Task<bool> UpdateAsync(UserRole userRole)
            => await _business.SaveUserRoleAsync(userRole);

        public async Task<bool> DeleteAsync(int id)
        {
            var userRole = await _business.GetUserRoleAsync(id);
            if (userRole == null) return false;
            return await _business.DeleteUserRoleAsync(userRole);
        }
    }
}