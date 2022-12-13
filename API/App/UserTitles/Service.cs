using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace SimplifyStreaming.API.App.UserTitles
{
    public class UserTitleService : IUserTitleService
    {
        private readonly IUserTitleRepository _userTitleRepository;

        public UserTitleService(IUserTitleRepository userTitleRepository)
        {
            _userTitleRepository = userTitleRepository;
        }
        public async Task<bool> Delete(string userId, string titleId)
        {
            var userTitle = await GetUserTitle(userId, titleId);

            if (userTitle == null) return false;

            await _userTitleRepository.Delete(userTitle);

            return true;
        }

        public async Task<UserTitle?> GetUserTitle(string userId, string titleId)
        {
            return await _userTitleRepository.Get(userId, titleId);
        }

        public async Task<List<UserTitle>> GetUserTitles(string userId)
        {
            return await _userTitleRepository.Get(userId);
        }

        public async Task<UserTitle> Save(UserTitle userTitle)
        {
            return await _userTitleRepository.Save(userTitle);
        }
    }
}
