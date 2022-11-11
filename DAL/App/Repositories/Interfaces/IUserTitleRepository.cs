using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface IUserTitleRepository : ISaveEntityRepository<UserTitle>
    {
        Task<List<UserTitle>> Get(string userId);
    }
}
