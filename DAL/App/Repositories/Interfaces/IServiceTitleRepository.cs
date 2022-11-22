using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface IServiceTitleRepository : ISaveEntityRepository<ServiceTitle>
    {
        Task<List<ServiceTitle>> GetAll(Service id);
        Task<ServiceTitle?> Get(Service id, string titleId);
        Task<List<ServiceTitle>> GetStreamingServicesForTitle(string titleId);
    }
}
