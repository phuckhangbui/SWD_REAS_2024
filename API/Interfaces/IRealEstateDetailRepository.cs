using API.Entity;

namespace API.Interfaces
{
    public interface IRealEstateDetailRepository : IBaseRepository<RealEstateDetail>
    {
        Task<RealEstateDetail> GetRealEstateDetailAsync(int reasId);
    }
}
