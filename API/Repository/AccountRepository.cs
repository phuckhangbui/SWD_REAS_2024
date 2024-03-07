﻿using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
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
            return await _context.Account.AnyAsync(x => x.AccountEmail.ToLower() == email.ToLower() && x.RoleId == 3);
        }

        public async Task<bool> isEmailExistedCreateAccount(string email)
        {
            return await _context.Account.AnyAsync(x => x.AccountEmail.ToLower() == email.ToLower() && x.RoleId == 2);
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
                .SingleOrDefaultAsync(x => x.AccountEmail == email && x.RoleId == 3);

        public async Task<IEnumerable<AccountMemberDto>> GetMemberAccountsBySearch(AccountParams accountParams)
        {
            var getName = new GetStatusName();
            var query = _context.Account.AsQueryable();

            query = query.Where(a => a.RoleId == (int)RoleEnum.Member &&
            (accountParams.KeyWord == null || a.AccountEmail.ToLower().Contains(accountParams.KeyWord.ToLower()) ||
            a.AccountName.ToLower().Contains(accountParams.KeyWord.ToLower())));
            var result  = query.Select(x => new AccountMemberDto
            {
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Account_Status = getName.GetStatusAccountName(x.Account_Status),
                Date_Created = x.Date_Created,
            });

            result = result.OrderByDescending(a => a.Date_Created);

            return result.ToList();
        }

        public async Task<IEnumerable<AccountStaffDto>> GetStaffAccountsBySearch(AccountParams accountParams)
        {
            var getName = new GetStatusName();
            var query = _context.Account.AsQueryable();

            query = query.Where(a => a.RoleId == (int)RoleEnum.Staff &&
            (accountParams.KeyWord == null || a.AccountEmail.ToLower().Contains(accountParams.KeyWord.ToLower()) ||
            a.AccountName.ToLower().Contains(accountParams.KeyWord.ToLower())));

            var result = query.Select(x => new AccountStaffDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Account_Status = getName.GetStatusAccountName(x.Account_Status),
                Date_Created = x.Date_Created,
                Date_End = x.Date_End,
            });

            result = result.OrderByDescending(a => a.Date_Created);

            return result.ToList();
        }

        public async Task<IEnumerable<AccountStaffDto>> GetAllStaffAccounts()
        {
            var getName = new GetStatusName();
            PaginationParams paginationParams = new PaginationParams();
            var query = _context.Account.Where(x => x.RoleId.Equals((int)RoleEnum.Staff)).OrderByDescending(x => x.AccountId).Select(x => new AccountStaffDto
            {
                AccountId = x.AccountId,
                Username = x.Username,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Account_Status = getName.GetStatusAccountName(x.Account_Status),
                Date_Created = x.Date_Created,
                Date_End = x.Date_End,
            }).OrderByDescending(x =>x.Date_Created);
            return query.ToList();
        }

        public async Task<IEnumerable<AccountMemberDto>> GetAllMemberAccounts()
        {
            var getName = new GetStatusName();
            PaginationParams paginationParams = new PaginationParams();
            var query = _context.Account.Where(x => x.RoleId.Equals((int)RoleEnum.Member)).OrderByDescending(x => x.AccountId).Select(x => new AccountMemberDto
            {
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Account_Status = getName.GetStatusAccountName(x.Account_Status),
                Date_Created = x.Date_Created
            }).OrderByDescending(x => x.Date_Created);
            return query.ToList();
        }

        public async Task<MemberInformationDto> GetMemberAccountDetail(int id)
        {
            var getName = new GetStatusName();
            var account = _context.Account.Where(x => x.AccountId == id).Select(x => new MemberInformationDto
            {
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                AccountEmail = x.AccountEmail,
                Address = x.Address,
                Citizen_identification = x.Citizen_identification,
                PhoneNumber = x.PhoneNumber,
                Date_Created = x.Date_Created,
                Date_End = x.Date_End,
                Major = _context.Major.Where(y => y.MajorId == x.MajorId).Select(x => x.MajorName).FirstOrDefault(),
                Account_Status = getName.GetStatusAccountName(x.Account_Status)
            }).FirstOrDefault();

            return account;
        }

        public async Task<StaffInformationDto> GetStaffAccountDetail(int id)
        {
            var getName = new GetStatusName();
            var account = _context.Account.Where(x => x.AccountId == id).Select(x => new StaffInformationDto
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
                Account_Status = getName.GetStatusAccountName(x.Account_Status)
            }).FirstOrDefault();

            return account;
        }

        public async Task<bool> UpdateStatusAccount(ChangeStatusAccountParam changeStatusAccountDto)
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
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }  
        }

        public async Task<string> GetNameAccountByAccountIdAsync(int accountId)
        => await _context.Account
                .Where(x => x.AccountId == accountId).Select(x => x.AccountName).FirstOrDefaultAsync();

        public async Task<int> GetIdAccountToReceiveMoney()
        => await _context.Account.Where(x => x.AccountName.Equals("admin")).Select(x => x.AccountId).FirstOrDefaultAsync();

    }
}
