using API.DTOs;
using API.Entity;

namespace API.Interface.Service
{
    public interface IAuctionAccountingService
    {
        Task<AuctionAccounting> UpdateAuctionAccounting(AuctionDetailDto auctionDetailDto);
    }
}
