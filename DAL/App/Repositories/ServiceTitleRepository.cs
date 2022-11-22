using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.App.Repositories
{
    public class ServiceTitleRepository : BaseRepository, IServiceTitleRepository
    {
        private readonly ISaveEntityRepository<ServiceTitle> _saveEntityRepository;
        private readonly IDynamoDBContext _db;

        public ServiceTitleRepository(ISaveEntityRepository<ServiceTitle> saveEntityRepository, IDynamoDBContext db) : base(db)
        {
            _saveEntityRepository = saveEntityRepository;
            _db = db;
        }
        public async Task Delete(ServiceTitle serviceTitle)
        {
            await _saveEntityRepository.Delete(serviceTitle);
        }

        public async Task<List<ServiceTitle>> GetAll(Service id)
        {
            var serviceTitles = await GetDynamoQueryResults<ServiceTitle>(
                "PK",
                SKPrefix.SERVICE + id.ToString(),
                SKPrefix.SERVICE_TITLE
            );
            return serviceTitles;
        }

        public async Task<ServiceTitle?> Get(Service id, string titleId)
        {
            var serviceTitles = await GetDynamoQueryResults<ServiceTitle>("PK", SKPrefix.SERVICE + id.ToString(), SKPrefix.SERVICE_TITLE + titleId);
            return serviceTitles.FirstOrDefault();
        }

        public async Task<List<ServiceTitle>> GetStreamingServicesForTitle(string titleId)
        {
            var serviceTitles = await GetDynamoQueryResults<ServiceTitle>(
                Globals.GSI_1_INDEX,
                Globals.GSI_NAME,
                titleId,
                SKPrefix.SERVICE_TITLE + titleId
            );
            return serviceTitles;
        }

        public async Task<ServiceTitle> Save(ServiceTitle serviceTitle)
        {
            return await _saveEntityRepository.Save(serviceTitle);
        }
    }
}
