using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<PageList<RealEstateDto>> GetRealEstatesAsync(RealEstateParam realEstateParam);
        Task<ReasStatusDto> UpdateRealEstateStatusAsync(ReasStatusDto reasStatusDto);
		Task<bool> CheckRealEstateExist(int reasId);
        Task<PageList<RealEstateDto>> GetOwnerRealEstate(int idOwner);
        Task<PageList<RealEstateDto>> GetRealEstateOnGoing();
        Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoing();
        Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateDto searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetAllRealEstateOnRealEstatePage();
    }
}
