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
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IRuleRepository _ruleRepository;
        private readonly IAuctionRepository _auctionRepository;

        public BaseApiController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public BaseApiController(IRealEstateRepository realEstateRepository)
        {
            _realEstateRepository = realEstateRepository;
        }

        public BaseApiController(IRealEstateDetailRepository realEstateDetailRepository)
        {
            _realEstateDetailRepository = realEstateDetailRepository;
        }

        public BaseApiController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public BaseApiController(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public BaseApiController(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
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
