using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.ServiceTitles
{
    public interface IServiceTitleService
    {
        Task<List<ServiceTitle>> GetTitles(Service service);
        Task<ServiceTitle?> GetTitle(Service service, string titleId);
        Task<ServiceTitle> Save(ServiceTitle serviceTitle);
        Task<bool> Delete(Service service, string titleId);
    }
}
