using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<PageList<RealEstateDto>> GetRealEstatesAsync(RealEstateParam realEstateParam);
        Task<RealEstateDto> GetRealEstateAsync(int reasId);
        Task<RealEstateDto> GetRealEstateWithStatusAsync(int reasId, int status);
        Task<RealEstateDto> UpdateRealEstateStatusAsync(int reasId, int status);
		Task<bool> CheckRealEstateExist(int reasId);
	}
}
