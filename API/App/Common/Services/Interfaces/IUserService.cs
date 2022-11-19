using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Common.Services
{
    public interface IUserService
    {
        Task<User?> GetUser(string id);
        Task<User> Save(User user);
        Task<bool> Delete(string id);
    }
}
