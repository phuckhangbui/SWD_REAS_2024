using API.DTOs;
using API.Helper;

namespace API.Interfaces;

public interface IAdminRepository
{
    Task<PageList<AccountDto>> GetAccounts(AccountParams accountParams);
}