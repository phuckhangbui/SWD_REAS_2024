using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class RealEstateDetailRepository : BaseRepository<RealEstateDetail>, IRealEstateDetailRepository
    {
        public RealEstateDetailRepository(DataContext context) : base(context)
        {
        }
    }
}
