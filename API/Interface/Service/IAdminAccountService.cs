using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAdminAccountService : IBaseService<Account>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<AccountStaffDto>> GetStaffAccountBySearch(AccountParams accountParams);
        Task<PageList<AccountMemberDto>> GetMemberAccountBySearch(AccountParams accountParams);
        Task<PageList<AccountMemberDto>> GetMemberAccounts();
        Task<PageList<AccountStaffDto>> GetStaffAccounts();
        Task<StaffInformationDto> GetStaffDetail(int idAccount);
        Task<MemberInformationDto> GetMemberDetail(int idAccount);
        Task<bool> ChangeStatusAccount(ChangeStatusAccountParam statusAccountParam);
        Task<bool> CreateNewAccountForStaff(NewAccountParam accountParam);
    }
}
