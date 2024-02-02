using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public BaseApiController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected int? GetLoginAccountId()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected async Task<Account> GetLoginAccountAsync()
        {
            int? accountId = GetLoginAccountId();
            if (accountId != null)
            {
                return await _accountRepository.GetAccountByAccountIdAsync((int)accountId);
            }
            return null;
        }
    }
}
