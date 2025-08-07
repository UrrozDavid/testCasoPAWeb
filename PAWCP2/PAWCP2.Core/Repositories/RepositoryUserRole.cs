using PAWCP2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Repositories;

namespace PAWCP2.Core.Repositories
{
    public interface IRepositoryUserRole
    {
        Task<bool> UpsertAsync(UserRole entity, bool isUpdating);

        Task<bool> CreateAsync(UserRole entity);

        Task<bool> DeleteAsync(UserRole entity);

        Task<IEnumerable<UserRole>> ReadAsync();

        Task<UserRole> FindAsync(int id);

        Task<bool> UpdateAsync(UserRole entity);

        Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);

        Task<bool> ExistsAsync(UserRole entity);
        Task<bool> CheckBeforeSavingAsync(UserRole entity);
    }

    public class RepositoryUserRole : RepositoryBase<UserRole>, IRepositoryUserRole
    {
        public RepositoryUserRole(FoodbankContext context) : base(context) { }

        public async Task<bool> CheckBeforeSavingAsync(UserRole entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists)
            {
                // algo más
            }

            return await UpsertAsync(entity, exists);
        }
    }
}
