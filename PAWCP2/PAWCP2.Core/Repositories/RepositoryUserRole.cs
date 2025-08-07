using PAWCP2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Repositories;
using PAWCP2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Models.DTOs;

namespace PAWCP2.Core.Repositories
{
    public interface IRepositoryUserRole
    {
        Task<bool> UpsertAsync(UserRole entity, bool isUpdating);

        Task<bool> CreateAsync(UserRole entity);

        Task<bool> DeleteAsync(UserRole entity);

        Task<IEnumerable<UserRole>> ReadAsync();

        Task<UserRole> FindAsync(int userId);

        Task<bool> UpdateAsync(UserRole entity);

        Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);

        Task<bool> ExistsAsync(UserRole entity);
        Task<bool> CheckBeforeSavingAsync(UserRole entity);
        Task<int?> GetRoleAsync(int userId);

        Task<IEnumerable<UserRoleDto>> ReadDtoAsync();
    }

    public class RepositoryUserRole : RepositoryBase<UserRole>, IRepositoryUserRole
    {
        public RepositoryUserRole(FoodbankContext context) : base(context) { }

        public async Task<bool> ExistsAsync(UserRole entity)
        => await _context.UserRoles.AnyAsync(userRole => userRole.UserId == entity.UserId);

        public async Task<bool> UpsertAsync(UserRole entity, bool isUpdating)
        {
            if (!isUpdating)
            {
                await _context.UserRoles.AddAsync(entity);
                return await _context.SaveChangesAsync() > 0;
            }

            // isUpdating == true  -> replace (delete + insert) porque RoleId es parte de la PK
            // y si se elimina da error.
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.UserRoles
                    .Where(ur => ur.UserId == entity.UserId)
                    .ToListAsync();

                if (existing.Count > 0)
                {
                    _context.UserRoles.RemoveRange(existing);
                    await _context.SaveChangesAsync();
                }

                await _context.UserRoles.AddAsync(new UserRole
                {
                    UserId = entity.UserId,
                    RoleId = entity.RoleId,
                    Description = entity.Description
                });

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CheckBeforeSavingAsync(UserRole entity)
        {
            var exists = await ExistsAsync(entity);

            return await UpsertAsync(entity, exists);
        }
    
        public async Task<IEnumerable<UserRole>> ReadAsync()
        {
            return await _context.UserRoles
                        .Include(ur => ur.Role)
                        .Include(ur => ur.User)
                        .ToListAsync();
        }

        public async Task<int?> GetRoleAsync(int userId)
        {
            return await _context.UserRoles
                                 .Where(ur => ur.UserId == userId)
                                 .Select(ur => ur.RoleId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserRoleDto>> ReadDtoAsync() =>
            await _context.UserRoles
                  .AsNoTracking()
                  .Select(ur => new UserRoleDto
                  {
                      UserId = ur.UserId,
                      RoleId = ur.RoleId,
                      RoleName = ur.Role.RoleName,
                      UserName = ur.User.Username
                  })
                  .ToListAsync();

    }
}
