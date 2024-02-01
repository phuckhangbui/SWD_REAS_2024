using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class RealEstateRepository : BaseRepository<RealEstate>, IRealEstateRepository
    {
        public RealEstateRepository(DataContext context) : base(context)
        {
        }
    }
}
