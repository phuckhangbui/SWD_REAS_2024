namespace API.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IAsyncEnumerable<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(ICollection<T> entity);
    }
}
