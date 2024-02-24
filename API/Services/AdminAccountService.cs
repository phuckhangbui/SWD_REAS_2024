﻿using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using API.Param.Enums;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace API.Services
{
    public class AdminAccountService : BaseService<Entity.Account>, IAdminAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AdminAccountService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _accountRepository = accountRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<bool> ChangeStatusAccount(ChangeStatusAccountParam statusAccountParam)
        {
            try
            {
                bool flag = await _accountRepository.UpdateStatusAccount(statusAccountParam);
                if (flag)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateNewAccountForStaff(NewAccountParam accountParam)
        {
            try
            {
                var newaccount = new Entity.Account();
                using var hmac = new HMACSHA512();
                newaccount.Username = accountParam.Username;
                newaccount.AccountEmail = accountParam.AccountEmail;
                newaccount.Address = accountParam.Address;
                newaccount.AccountName = accountParam.AccountName;
                newaccount.Citizen_identification = accountParam.Citizen_identification;
                newaccount.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(accountParam.PasswordHash));
                newaccount.PasswordSalt = hmac.Key;
                newaccount.PhoneNumber = accountParam.PhoneNumber;
                newaccount.RoleId = 2;
                newaccount.Date_Created = DateTime.UtcNow;
                newaccount.Date_End = DateTime.MaxValue;
                newaccount.Account_Status = 1;
                bool flag = await _accountRepository.CreateAsync(newaccount);
                if(flag)
                {
                    SendMailNewStaff.SendEmailWhenCreateNewStaff(accountParam.AccountEmail, accountParam.Username, accountParam.PasswordHash, accountParam.AccountName);
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<PageList<AccountMemberDto>> GetMemberAccountBySearch(AccountParams accountParams)
        {
            accountParams.RoleId = (int)RoleEnum.Member;
            var accounts = await _accountRepository.GetMemberAccountsBySearch(accountParams);
            return accounts;
        }
        public async Task<PageList<AccountMemberDto>> GetMemberAccounts()
        {
            var list_account = await _accountRepository.GetAllMemberAccounts();
            return list_account;
        }

        public async Task<MemberInformationDto> GetMemberDetail(int idAccount)
        {
            var accountMember = await _accountRepository.GetMemberAccountDetail(idAccount);
            return accountMember;
        }

        public async Task<PageList<AccountStaffDto>> GetStaffAccountBySearch(AccountParams accountParams)
        {
            accountParams.RoleId = (int)RoleEnum.Staff;
            var accounts = await _accountRepository.GetStaffAccountsBySearch(accountParams);
            return accounts;
        }

        public async Task<PageList<AccountStaffDto>> GetStaffAccounts()
        {
            var list_account = await _accountRepository.GetAllStaffAccounts();
            return list_account;
        }

        public async Task<StaffInformationDto> GetStaffDetail(int idAccount)
        {
            var accountStaff = await _accountRepository.GetStaffAccountDetail(idAccount);
            return accountStaff;
        }
    }
}
