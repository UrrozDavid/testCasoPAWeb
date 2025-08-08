using PAWCP2.Core.Models;
using PAWCP2.Core.Manager;
using PAWCP2.Models.Entities;

namespace PAWCP2.Mvc.Service
{
    public interface IUserService
    {
        // CRUD
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<int?> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

        Task<User?> AuthenticateAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IManagerUser _business;

        public UserService(IManagerUser business)
        {
            _business = business;
        }

        // ----------------- CRUD -----------------
        public async Task<List<User>> GetAllAsync() => (await _business.GetAllUserAsync()).ToList();
        public async Task<User?> GetByIdAsync(int id) => await _business.GetUserAsync(id);

        public async Task<int?> CreateAsync(User user)
        {
            var sucess = await _business.SaveUserAsync(user);
            return sucess ? user.UserId : null;
        }

        public async Task<bool> UpdateAsync(User user)
            => await _business.SaveUserAsync(user);

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _business.GetUserAsync(id);
            if (user == null) return false;
            return await _business.DeleteUserAsync(user);
        }

        // ----------------- AUTH -----------------
        public async Task<User?> AuthenticateAsync(string username, string password) => await _business.AuthenticateAsync(username, password);
    }
}