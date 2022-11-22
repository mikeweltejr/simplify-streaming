using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace SimplifyStreaming.API.App.ServiceTitles
{
    public class ServiceTitleService : IServiceTitleService
    {
        private readonly IServiceTitleRepository _repository;

        public ServiceTitleService(IServiceTitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Delete(Service service, string titleId)
        {
            var serviceTitle = await GetTitle(service, titleId);

            if(serviceTitle == null) return false;

            await _repository.Delete(serviceTitle);
            return true;
        }

        public async Task<ServiceTitle?> GetTitle(Service service, string titleId)
        {
            return await _repository.Get(service, titleId);
        }

        public async Task<List<ServiceTitle>> GetTitles(Service service)
        {
            return await _repository.GetAll(service);
        }

        public async Task<ServiceTitle> Save(ServiceTitle serviceTitle)
        {
            return await _repository.Save(serviceTitle);
        }
    }
}
