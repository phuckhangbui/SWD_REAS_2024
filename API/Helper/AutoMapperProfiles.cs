using API.DTOs;
using API.Entity;
using AutoMapper;

namespace API.Helper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<RegisterDto, Account>();
        CreateMap<NewAccountDto, Account>();
        CreateMap<RealEstate, RealEstateDto>();
        CreateMap<RealEstateDetail, RealEstateInfoDto>();
        CreateMap<RealEstatePhoto, RealEstatePhotoDto>();
        CreateMap<Auction, AuctionDto>()
            .ForMember(dest => dest.AccountCreateName, opt => opt.MapFrom(src => src.AccountCreateName));
        CreateMap<Entity.Task, TaskDto>();
    }
}