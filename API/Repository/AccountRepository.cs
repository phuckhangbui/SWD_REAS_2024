using API.Data;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> isEmailExisted(string email)
        {
            return await _context.Account.AnyAsync(x => x.AccountEmail.ToLower() == email.ToLower());
        }

        public async Task<bool> isUserNameExisted(string userName)
        {
            return await _context.Account.AnyAsync(x => x.Username.ToLower() == userName.ToLower());
        }

        public async Task<Account> GetAccountByUsernameAsync(string username) => await _context.Account
                .SingleOrDefaultAsync(x => x.Username == username);

        public async Task<Account> GetAccountByAccountIdAsync(int accountId) => await _context.Account
                .SingleOrDefaultAsync(x => x.AccountId == accountId);

        public async Task<Account> GetAccountByEmailAsync(string email) => await _context.Account
                .SingleOrDefaultAsync(x => x.AccountEmail == email);



    }
}
