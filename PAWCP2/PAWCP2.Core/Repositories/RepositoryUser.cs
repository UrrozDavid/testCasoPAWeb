using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Models;
using PAWCP2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Repositories;

namespace PAWCP2.Core.Repositories
{
    public interface IRepositoryUser
    {
        Task<bool> UpsertAsync(User entity, bool isUpdating);

        Task<bool> CreateAsync(User entity);

        Task<bool> DeleteAsync(User entity);

        Task<IEnumerable<User>> ReadAsync();

        Task<User> FindAsync(int id);

        Task<bool> UpdateAsync(User entity);

        Task<bool> UpdateManyAsync(IEnumerable<User> entities);

        Task<bool> ExistsAsync(User entity);
        Task<bool> CheckBeforeSavingAsync(User entity);

        Task<User> GetByUsernameAsync(string username);
    }

    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        public RepositoryUser(FoodbankContext context) : base(context) { }

        public async Task<bool> CheckBeforeSavingAsync(User entity)
        {
            var exists = await ExistsAsync(entity);
            if (exists)
            {
                // algo más
            }

            return await UpsertAsync(entity, exists);
        }
    
        public async Task<User> GetByUsernameAsync(string username) => await DbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

    }
}
