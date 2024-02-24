using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{

    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<PageList<AuctionDto>> GetAuctionsAsync(AuctionParam auctionParam);

        Task<PageList<AuctionDto>> GetAuctions(AuctionParam auctionParam);

        Task<bool> EditAuctionStatus(string autionId, string statusCode);
    }

}
