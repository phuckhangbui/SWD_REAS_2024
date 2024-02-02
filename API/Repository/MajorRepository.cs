using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class MajorRepository : BaseRepository<Major>, IMajorRepository
    {
        public MajorRepository(DataContext context) : base(context)
        {
        }
    }
}
