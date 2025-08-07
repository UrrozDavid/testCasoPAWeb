using PAWCP2.Core.Extensions;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Models.Entities;
using PAWCP2.Models.Enums;

namespace PAWCP2.Core.Manager
{
    public interface IManagerUser
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<bool> SaveUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserAsync(int id);

    }

    public class ManagerUser(IRepositoryUser repositoryUser) : IManagerUser
    {
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await repositoryUser.ReadAsync();
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            bool isUpdate = user.UserId > 0;
            user.AddAudit("system");
            user.AddLogging(isUpdate ? PAWCP2.Models.Enums.LoggingType.Update : PAWCP2.Models.Enums.LoggingType.Create);

            if (isUpdate)
            {
                var existing = await repositoryUser.FindAsync(user.UserId);
                if (existing == null) return false;

                existing.UserId = user.UserId;
                existing.Username = user.Username;
                existing.Email = user.Email;
                existing.IsActive = user.IsActive;

                return await repositoryUser.UpdateAsync(existing);
            }
            else
            {
                return await repositoryUser.CreateAsync(user);
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            return await repositoryUser.DeleteAsync(user);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await repositoryUser.FindAsync(id);
        }
    }
}
