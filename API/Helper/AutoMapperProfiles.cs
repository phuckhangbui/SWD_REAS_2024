using API.DTOs;
using API.Entity;
using AutoMapper;

namespace API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, Account>();
            CreateMap<NewAccountDto, Account>();
            CreateMap<ChangeStatusAccountDto, Account>();
            CreateMap<RuleChangeContentDto, Rule>();
            CreateMap<AccountDto, Account>();
            CreateMap<AccountDto, AccountDto>();
            CreateMap<NewsDto, NewsDto>();
            CreateMap<News, NewsDto>();
            CreateMap<Rule, Rule>();
            CreateMap<RealEstate, RealEstateDto>();
            CreateMap<RealEstateDto, RealEstateDto>();
            CreateMap<RealEstatePhoto, RealEstatePhotoDto>();
            CreateMap<Auction, AuctionDto>()
                .ForMember(dest => dest.AccountCreateName, opt => opt.MapFrom(src => src.AccountCreateName));
        }
    }

}
