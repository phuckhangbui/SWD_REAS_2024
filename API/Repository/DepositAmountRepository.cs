using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class DepositAmountRepository : BaseRepository<DepositAmount>, IDepositAmountRepository
    {
        private readonly DataContext _context;
        public DepositAmountRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
