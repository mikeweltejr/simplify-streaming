using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.App.Repositories
{
    public class SaveEntityRepository<T> : ISaveEntityRepository<T>
    {
        private readonly IDynamoDBContext _db;

        public SaveEntityRepository(IDynamoDBContext db)
        {
            _db = db;
        }
        public async Task Delete(T entity)
        {
            await _db.DeleteAsync(entity, Globals.DB_CONFIG);
        }

        public async Task<T> Save(T entity)
        {
            await _db.SaveAsync(entity, Globals.DB_CONFIG);
            return entity;
        }
    }
}
