using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }
    }
}
