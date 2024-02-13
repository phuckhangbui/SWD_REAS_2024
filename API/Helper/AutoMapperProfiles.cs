using API.DTOs;
using API.Entity;
using AutoMapper;

namespace API.Helper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<RealEstate, ListRealEstateDto>();
        CreateMap<Auction, AuctionDto>();
    }
}