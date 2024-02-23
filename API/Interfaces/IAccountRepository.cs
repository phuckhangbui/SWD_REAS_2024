using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<bool> isEmailExisted(string email);
        Task<int> GetIdAccountToReceiveMoney();
        Task<bool> isUserNameExisted(string userName);
        Task<Account> GetAccountByUsernameAsync(string username);
        Task<Account> GetAccountByAccountIdAsync(int accountId);
        Task<string> GetNameAccountByAccountIdAsync(int accountId);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<PageList<AccountDto>> GetAccountsBySearch(AccountParams accountParams);
        Task<PageList<AccountDto>> GetAllStaffAccounts();
        Task<PageList<AccountDto>> GetAllMemberAccounts();
        Task<UserInformationDto> GetAccountDetail(int id);
        Task<ChangeStatusAccountDto> UpdateStatusAccount(ChangeStatusAccountDto changeStatusAccountDto);
    }
}
