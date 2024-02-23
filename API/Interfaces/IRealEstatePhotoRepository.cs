using API.Entity;

namespace API.Interfaces
{
    public interface IRealEstatePhotoRepository : IBaseRepository<RealEstatePhoto>
    {
        string GetBestUriPhoto(int id);
    }
}
