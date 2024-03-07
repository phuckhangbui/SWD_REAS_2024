using API.DTOs;
using API.Helper;
using API.Param;


namespace API.Interface.Service
{
    public interface IAuctionService
    {
        Task<PageList<AuctionDto>> GetRealEstates(AuctionParam auctionParam);
        Task<PageList<AuctionDto>> GetAuctions(AuctionParam auctionParam);
        Task<bool> ToggleAuctionStatus(string auctionId, string statusCode);
        Task<PageList<AuctionDto>> GetAuctionHisotoryForOwner(AuctionHistoryParam auctionAccountingParam);
        Task<PageList<AuctionDto>> GetAuctionHisotoryForAttender(AuctionHistoryParam auctionAccountingParam);

        Task<PageList<AuctionDto>> GetNotyetAndOnGoingAuction(AuctionParam auctionParam);

    }
}
