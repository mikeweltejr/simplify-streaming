using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface ITitleRepository : ISaveEntityRepository<Title>
    {
        Task<Title?> Get(string id);
    }
}
