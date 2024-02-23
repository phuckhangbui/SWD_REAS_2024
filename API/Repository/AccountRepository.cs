using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PageList<AccountDto>> GetAccountsBySearch(AccountParams accountParams)
        {
            var query = _context.Account.AsQueryable();

            query = query.Where(a => a.RoleId == accountParams.RoleID);

            if (accountParams.Month.HasValue)
            {
                query = query.Where(x => x.Date_Created.Month == accountParams.Month);
            }

            // Filtering by year (optional)
            if (accountParams.Year.HasValue)
            {
                query = query.Where(x => x.Date_Created.Year == accountParams.Year);
            }

            query = query.OrderByDescending(a => a.Date_Created);

            return await PageList<AccountDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<AccountDto>(_mapper.ConfigurationProvider),
                accountParams.PageNumber,
                accountParams.PageSize);
        }

        public async Task<PageList<AccountDto>> GetAllStaffAccounts()
        {
            PaginationParams paginationParams = new PaginationParams();
            var query = _context.Account.Where(x => x.RoleId.Equals((int)RoleEnum.Staff)).OrderByDescending(x => x.AccountId).Select(x => new AccountListDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                PhoneNumber = x.PhoneNumber,
                Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status,
                Date_Created = x.Date_Created
            });
            return await PageList<AccountDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<AccountDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }

        public async Task<PageList<AccountDto>> GetAllMemberAccounts()
        {
            PaginationParams paginationParams = new PaginationParams();
            var query = _context.Account.Where(x => x.RoleId.Equals((int)RoleEnum.Member)).OrderByDescending(x => x.AccountId).Select(x => new AccountListDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                PhoneNumber = x.PhoneNumber,
                Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status,
                Date_Created = x.Date_Created
            });
            return await PageList<AccountDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<AccountDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }

        public async Task<UserInformationDto> GetAccountDetail(int id)
        {
            var account = _context.Account.Where(x => x.AccountId == id).Select(x => new UserInformationDto
            {
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Address = x.Address,
                Citizen_identification = x.Citizen_identification,
                PhoneNumber = x.PhoneNumber,
                Username = x.Username,
                Date_Created = x.Date_Created,
                Date_End = x.Date_End,
                Major = _context.Major.Where(y => y.MajorId == x.MajorId).Select(x => x.MajorName).FirstOrDefault(),
                Role = _context.Role.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                Account_Status = x.Account_Status
            }).FirstOrDefault();

            return account;
        }

        public async Task<ChangeStatusAccountDto> UpdateStatusAccount(ChangeStatusAccountDto changeStatusAccountDto)
        {
            var account = await _context.Account.Where(x => x.AccountId == changeStatusAccountDto.AccountId).FirstOrDefaultAsync();
            if (account != null)
            {
                account.Account_Status = changeStatusAccountDto.AccountStatus;
                if (changeStatusAccountDto.AccountStatus == 0)
                {
                    account.Date_End = DateTime.UtcNow;
                }
                else
                {
                    account.Date_End = DateTime.MaxValue;
                }
                try
                {
                    await UpdateAsync(account);
                    return changeStatusAccountDto;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }  
        }

        public async Task<string> GetNameAccountByAccountIdAsync(int accountId)
        => await _context.Account
                .Where(x => x.AccountId == accountId).Select(x => x.AccountName).FirstOrDefaultAsync();
    }
}
