using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface IServiceTitleRepository : ISaveEntityRepository<ServiceTitle>
    {
        Task<List<ServiceTitle>> Get(Service id);
        Task<List<ServiceTitle>> GetStreamingServicesForTitle(string titleId);
    }
}
