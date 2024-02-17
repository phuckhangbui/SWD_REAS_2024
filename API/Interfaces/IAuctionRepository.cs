using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<PageList<AuctionDto>> GetAuctions(AuctionParam auctionParam);

        System.Threading.Tasks.Task EditAuctionStatus(string autionId, string statusCode)
    }
}
