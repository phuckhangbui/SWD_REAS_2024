using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class MoneyTransactionRepository : BaseRepository<MoneyTransaction>, IMoneyTransactionRepository
    {
        private readonly DataContext _dataContext;
        public MoneyTransactionRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }
    }
}
