using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.App.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ISaveEntityRepository<User> _saveEntityRepository;
        private readonly IDynamoDBContext _db;

        public UserRepository(ISaveEntityRepository<User> saveEntityRepository, IDynamoDBContext db) : base(db)
        {
            _saveEntityRepository = saveEntityRepository;
            _db = db;
        }

        public async Task Delete(User user)
        {
            await _saveEntityRepository.Delete(user);
        }

        public async Task<User?> Get(string id)
        {
            var results = await GetDynamoQueryResults<User>("PK", id, SKPrefix.USER + id);
            return results.FirstOrDefault();
        }

        public async Task<User> Save(User user)
        {
            return await _saveEntityRepository.Save(user);
        }
    }
}
