using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
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
        Task<PageList<AccountMemberDto>> GetMemberAccountsBySearch(AccountParams accountParams);
        Task<PageList<AccountStaffDto>> GetStaffAccountsBySearch(AccountParams accountParams);
        Task<PageList<AccountStaffDto>> GetAllStaffAccounts();
        Task<PageList<AccountMemberDto>> GetAllMemberAccounts();
        Task<StaffInformationDto> GetStaffAccountDetail(int id);
        Task<MemberInformationDto> GetMemberAccountDetail(int id);
        Task<bool> UpdateStatusAccount(ChangeStatusAccountParam changeStatusAccountDto);
    }
}
