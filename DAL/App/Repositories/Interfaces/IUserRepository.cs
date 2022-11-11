using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface IUserRepository : ISaveEntityRepository<User>
    {
        Task<User?> Get(string id);
    }
}
