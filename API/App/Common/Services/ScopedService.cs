namespace SimplifyStreaming.API.App.Common.Services
{
    public class ScopedService<T> : IScopedService<T>
    {
        private T? _entity;

        public T? Get()
        {
            return _entity;
        }

        public void Set(T entity)
        {
            _entity = entity;
        }
    }
}
