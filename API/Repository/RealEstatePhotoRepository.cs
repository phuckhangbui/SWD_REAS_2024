using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class RealEstatePhotoRepository : BaseRepository<RealEstatePhoto>, IRealEstatePhotoRepository
    {
        public RealEstatePhotoRepository(DataContext context) : base(context)
        {
        }
    }
}
