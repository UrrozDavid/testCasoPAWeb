using PAWCP2.Core.Extensions;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Models.Enums;

namespace PAWCP2.Core.Manager
{
    public interface IManagerUserRole
    {
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
        Task<bool> SaveUserRoleAsync(UserRole userRole);
        Task<bool> DeleteUserRoleAsync(UserRole userRole);
        Task<UserRole> GetUserRoleAsync(int id);

    }

    public class ManagerUserRole(IRepositoryUserRole repositoryUserRole) : IManagerUserRole
    {
        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await repositoryUserRole.ReadAsync();
        }

        public async Task<bool> SaveUserRoleAsync(UserRole userRole)
        {
            bool isUpdate = userRole.UserId > 0;
            userRole.AddAudit("system");
            userRole.AddLogging(isUpdate ? PAWCP2.Models.Enums.LoggingType.Update : PAWCP2.Models.Enums.LoggingType.Create);

            if (isUpdate)
            {
                var existing = await repositoryUserRole.FindAsync(userRole.UserId);
                if (existing == null) return false;

                existing.UserId = userRole.UserId;
                existing.Role = userRole.Role;
                existing.Description = userRole.Description;

                return await repositoryUserRole.UpdateAsync(existing);
            }
            else
            {
                return await repositoryUserRole.CreateAsync(userRole);
            }
        }

        public async Task<bool> DeleteUserRoleAsync(UserRole userRole)
        {
            return await repositoryUserRole.DeleteAsync(userRole);
        }

        public async Task<UserRole> GetUserRoleAsync(int id)
        {
            return await repositoryUserRole.FindAsync(id);
        }
    }
}
