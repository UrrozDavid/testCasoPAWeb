using PAWCP2.Core.Models;
using PAWCP2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Repositories;

namespace PAWCP2.Core.Repositories;

public interface IRepositoryRole
{
    Task<bool> UpsertAsync(Role entity, bool isUpdating);

    Task<bool> CreateAsync(Role entity);

    Task<bool> DeleteAsync(Role entity);

    Task<IEnumerable<Role>> ReadAsync();

    Task<Role> FindAsync(int id);

    Task<bool> UpdateAsync(Role entity);

    Task<bool> UpdateManyAsync(IEnumerable<Role> entities);

    Task<bool> ExistsAsync(Role entity);
    Task<bool> CheckBeforeSavingAsync(Role entity);
}

public class RepositoryRole : RepositoryBase<Role>, IRepositoryRole 
{
    public RepositoryRole(FoodbankContext context) : base(context) { }

    public async Task<bool> CheckBeforeSavingAsync(Role entity)
    {
        var exists = await ExistsAsync(entity);
        if (exists)
        {
            // algo más
        }

        return await UpsertAsync(entity, exists);
    }
}
