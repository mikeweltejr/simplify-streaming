namespace SimplifyStreaming.API.App.Common.Services
{
    public interface IScopedService<T>
    {
        T? Get();
        void Set(T entity);
    }
}
