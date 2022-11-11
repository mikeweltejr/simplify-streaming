using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.App.Repositories
{
    public class UserTitleRepository : BaseRepository, IUserTitleRepository
    {
        private readonly ISaveEntityRepository<UserTitle> _saveEntityRepository;
        private readonly IDynamoDBContext _db;

        public UserTitleRepository(ISaveEntityRepository<UserTitle> saveEntityRepository, IDynamoDBContext db) : base(db)
        {
            _saveEntityRepository = saveEntityRepository;
            _db = db;
        }

        public async Task Delete(UserTitle userTitle)
        {
            await _saveEntityRepository.Delete(userTitle);
        }

        public async Task<List<UserTitle>> Get(string userId)
        {
            var userTitles = await GetDynamoQueryResults<UserTitle>("PK", userId, SKPrefix.USER_TITLE);
            return userTitles;
        }

        public async Task<UserTitle> Save(UserTitle userTitle)
        {
            return await _saveEntityRepository.Save(userTitle);
        }
    }
}
