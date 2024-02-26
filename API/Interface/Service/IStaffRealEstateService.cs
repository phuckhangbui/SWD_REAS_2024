using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IStaffRealEstateService : IBaseService<RealEstate>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<RealEstateDto>> GetRealEstateOnGoingByStaff();
        Task<PageList<RealEstateDto>> GetRealEstateOnGoingByStaffBySearch(SearchRealEstateParam searchRealEstateDto);
        Task<RealEstateDetailDto> GetRealEstateOnGoingDetailByStaff(int id);
        Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingByStaff();
        Task<PageList<RealEstateDto>> GetRealEstateExceptOnGoingByStaffBySearch(SearchRealEstateParam searchRealEstateDto);
        Task<RealEstateDetailDto> GetRealEstateExceptOnGoingDetailByStaff(int id);
        Task<bool> UpdateStatusRealEstateByStaff(ReasStatusParam reasStatusDto);
    }
}
