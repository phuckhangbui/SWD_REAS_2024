using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Interface.Service
{
    public interface IRealEstateService : IBaseService<RealEstate>
    {
        Task<PageList<RealEstateDto>> ListRealEstate();
        Task<PageList<RealEstateDto>> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto);
        Task<RealEstateDetailDto> ViewRealEstateDetail(int id);
    }
}
