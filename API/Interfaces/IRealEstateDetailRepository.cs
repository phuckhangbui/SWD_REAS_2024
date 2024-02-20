using API.DTOs;
using API.Entity;

namespace API.Interfaces
{
    public interface IRealEstateDetailRepository : IBaseRepository<RealEstateDetail>
    {
        Task<RealEstateInfoDto> GetRealEstateDetailAsync(int reasId);
    }
}
