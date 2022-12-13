using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.UserTitles
{
    public interface IUserTitleService
    {
        Task<List<UserTitle>> GetUserTitles(string userId);
        Task<UserTitle?> GetUserTitle(string userId, string titleId);
        Task<UserTitle> Save(UserTitle userTitle);
        Task<bool> Delete(string userId, string titleId);
    }
}
