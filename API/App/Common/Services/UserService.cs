using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace SimplifyStreaming.API.App.Common.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IScopedService<User> _userScopedService;

        public UserService(
            IUserRepository userRepository,
            IScopedService<User> userScopedService
        )
        {
            _userRepository = userRepository;
            _userScopedService = userScopedService;
        }
        public async Task<User?> GetUser(string id)
        {
            var user = _userScopedService.Get();

            if (user == null)
                user = await _userRepository.Get(id);

            return user;
        }

        public async Task<User> Save(User user)
        {
            return await _userRepository.Save(user);
        }

        public async Task<bool> Delete(string id)
        {
            var user = await GetUser(id);

            if (user == null) return false;

            await _userRepository.Delete(user);

            return true;
        }
    }
}
