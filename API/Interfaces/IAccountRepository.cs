using API.Entity;

namespace API.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<bool> isEmailExisted(string email);
        Task<bool> isUserNameExisted(string userName);
        Task<Account> GetAccountByUsernameAsync(string username);
        Task<Account> GetAccountByAccountIdAsync(int accountId);
    }
}
