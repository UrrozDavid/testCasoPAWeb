using PAWCP2.Core.Models;
using PAWCP2.Core.Manager;
using PAWCP2.Models.Entities;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAWCP2.Models.DTOs;

namespace PAWCP2.Mvc.Service
{
    public interface IUserRolesService
    {
        // CRUD
        Task<List<UserRoleDto>> GetAllAsync();
        Task<UserRoleDto?> GetByIdAsync(int userId, int roleId);
        Task<bool> SaveAsync(IEnumerable<UserRole> userRole);
        Task<bool> DeleteAsync(int userId, int roleId);
    }

    public class UserRolesService(IRestProvider restProvider) : IUserRolesService
    {
        // ----------------- CRUD -----------------
        public async Task<List<UserRoleDto>> GetAllAsync()
        {
            var result = await restProvider.GetAsync("https://localhost:7099/UserRoles/", null);
            var userRoles = await JsonProvider.DeserializeAsync<List<UserRoleDto>>(result);
            return userRoles;
        }

        public async Task<UserRoleDto?> GetByIdAsync(int userId, int roleId) 
        {
            var result = await restProvider.GetAsync("https://localhost:7099/UserRoles/", $"{userId}");
            var userRole = await JsonProvider.DeserializeAsync<UserRoleDto>(result);
            return userRole;
        }

        public async Task<bool> SaveAsync(IEnumerable<UserRole> userRole)
        {
            var payload = userRole.Select(x => new {
                userId = x.UserId,
                roleId = x.RoleId,
                description = x.Description
            });

            var content = JsonProvider.Serialize(payload);

            var result = await restProvider.PostAsync("https://localhost:7099/UserRoles", content);
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int roleId)
        {
            var result = await restProvider.DeleteAsync("https://localhost:7099/UserRoles/", $"{userId}/${roleId}");
            return true;
        }
    }
}