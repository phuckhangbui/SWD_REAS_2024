namespace API.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        ICollection<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(ICollection<T> entity);
    }
}
