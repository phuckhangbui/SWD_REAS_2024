using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IAsyncEnumerable<T>> GetAllAsync()
        {
            return (IAsyncEnumerable<T>)await _context.Set<T>().ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ICollection<T> entity)
        {
            try
            {
                foreach (T t in entity)
                {
                    _context.Set<T>().Remove(t);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public async Task UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
