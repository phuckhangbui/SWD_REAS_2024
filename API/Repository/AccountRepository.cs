using API.Data;
using API.Entity;
using API.Interfaces;

namespace API.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(DataContext context) : base(context)
        {
        }
    }
}
