using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.App.Repositories
{
    public class TitleRepository : BaseRepository, ITitleRepository
    {
        private readonly ISaveEntityRepository<Title> _saveEntityRepository;
        private readonly IDynamoDBContext _db;

        public TitleRepository(ISaveEntityRepository<Title> saveEntityRepository, IDynamoDBContext db) : base(db)
        {
            _saveEntityRepository = saveEntityRepository;
            _db = db;
        }

        public async Task Delete(Title title)
        {
            await _saveEntityRepository.Delete(title);
        }

        public async Task<Title?> Get(string id)
        {
            var titles = await GetDynamoQueryResults<Title>("PK", id, SKPrefix.TITLE + id);
            return titles?.FirstOrDefault();
        }

        public async Task<Title> Save(Title title)
        {
            return await _saveEntityRepository.Save(title);
        }
    }
}
