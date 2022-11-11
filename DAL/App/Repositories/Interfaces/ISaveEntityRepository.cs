namespace DynamoDB.DAL.App.Repositories.Interfaces
{
    public interface ISaveEntityRepository<T>
    {
        Task<T> Save(T entity);
        Task Delete(T entity);
    }
}
