using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<bool> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto);
        Task<bool> CheckRealEstateExist(int reasId);
        Task<PageList<RealEstateDto>> GetOwnerRealEstate(int idOwner);
        Task<PageList<RealEstateDto>> GetOwnerRealEstateBySearch(int idOwner, SearchRealEstateParam searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetRealEstateOnGoing();
        Task<PageList<RealEstateDto>> GetRealEstateOnGoingBySearch(SearchRealEstateParam searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingBySearch(SearchRealEstateParam searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoing();
        Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateParam searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetAllRealEstateOnRealEstatePage();
        RealEstate GetRealEstate(int id);
    }
}
