using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Repository;

namespace API.Interface.Service
{
    public interface IAdminRealEstateService : IBaseService<RealEstateRepository>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<RealEstateDto>> GetAllRealEstatesBySearch(SearchRealEstateParam searchRealEstateParam);
        Task<PageList<RealEstateDto>> GetAllRealEstatesPendingBySearch(SearchRealEstateParam searchRealEstateParam);
        Task<RealEstateDetailDto> GetRealEstatePendingDetail(int reasId);
        Task<RealEstateDetailDto> GetRealEstateAllDetail(int reasId);
        Task<bool> BlockRealEstate(int reasId);
        Task<bool> UnblockRealEstate(int reasId);
        Task<PageList<RealEstateDto>> GetRealEstateOnGoingByAdmin();
        Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingByAdmin();
        Task<bool> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusParam);
    }
}
