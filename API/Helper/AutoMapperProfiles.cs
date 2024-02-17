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
            CreateMap<Auction, AuctionDto>(); ;
        }
    }
}
