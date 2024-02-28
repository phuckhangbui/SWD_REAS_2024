using API.DTOs;
using API.Entity;

namespace API.Interface.Repository
{
    public interface IRealEstateDetailRepository : IBaseRepository<RealEstateDetail>
    {
        Task<RealEstateDetailDto> GetRealEstateDetail(int id);
    }
}
