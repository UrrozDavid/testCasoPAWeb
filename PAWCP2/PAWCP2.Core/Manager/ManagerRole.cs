using PAWCP2.Core.Extensions;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Models.Entities;
using PAWCP2.Models.Enums;

namespace PAWCP2.Core.Manager
{
    public interface IManagerRole
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> SaveRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(Role role);
        Task<Role> GetRoleAsync(int id);

    }

    public class ManagerRole(IRepositoryRole repositoryRole) : IManagerRole
    {
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await repositoryRole.ReadAsync();
        }

        public async Task<bool> SaveRoleAsync(Role role)
        {
            bool isUpdate = role.RoleId> 0;
            role.AddAudit("system");
            role.AddLogging(isUpdate ? PAWCP2.Models.Enums.LoggingType.Update : PAWCP2.Models.Enums.LoggingType.Create);

            if (isUpdate)
            {
                var existing = await repositoryRole.FindAsync(role.RoleId);
                if (existing == null) return false;

                existing.RoleName = role.RoleName;
                existing.Description = role.Description;

                return await repositoryRole.UpdateAsync(existing);
            }
            else
            {
                return await repositoryRole.CreateAsync(role);
            }
        }

        public async Task<bool> DeleteRoleAsync(Role role)
        {
            return await repositoryRole.DeleteAsync(role);
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            return await repositoryRole.FindAsync(id);
        }
    }
}
