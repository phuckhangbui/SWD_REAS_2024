using API.DTOs;
using API.Helper;

namespace API.Interfaces;

public interface IAdminRepository
{
    Task<PageList<AccountDto>> GetAccounts(AccountParams accountParams);
    Task<PageList<ListRealEstateDto>> GetRealEstates(ReasParams reasParams);
    Task<PageList<AuctionDto>> GetAuctions(AuctionParams auctionParams);
    Task<AuctionMoneyDto> GetAuctionMoney();
}