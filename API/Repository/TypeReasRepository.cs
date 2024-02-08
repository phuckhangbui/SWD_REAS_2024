using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class TypeReasRepository : BaseRepository<Type_REAS>, ITypeReasRepository
    {
        public TypeReasRepository(DataContext context) : base(context)
        {
        }
    }
}
