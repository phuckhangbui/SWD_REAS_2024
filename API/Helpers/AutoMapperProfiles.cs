using API.DTOs;
using API.Entity;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, Account>();
            CreateMap<RealEstate, RealEstateDto>();
            CreateMap<RealEstateDetail, RealEstateInfoDto>();
        }
    }
}
