using API.Data;
using API.Interfaces;

namespace API.Controllers
{
    public class AuctionController : BaseApiController
    {
        private IAuctionRepository _auctionrepository;
        public AuctionController(IAuctionRepository auctionRepository) : base (auctionRepository)
        {
            _auctionrepository = auctionRepository;
        }

    }
}
