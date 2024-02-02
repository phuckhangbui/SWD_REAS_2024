using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        private readonly DataContext _dataContext;
        public AuctionRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }
    }
}
