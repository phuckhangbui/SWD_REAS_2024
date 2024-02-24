using API.Entity;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberRuleContract : BaseApiController
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;
        private const string BaseUri = "/api/home/"; 

        public MemberRuleContract(IRuleRepository ruleRepository, IAccountRepository account_repository)
        {
            _rule_repository = ruleRepository;
            _account_repository = account_repository;
        }

        [HttpGet(BaseUri + "real_estate/rule")]
        public async Task<ActionResult<Rule>> GetRuleContractWhenUserSignInAuction()
        {
            int userMember = GetIdMember(_account_repository);
            if (userMember != 0)
            {
                var rule = _rule_repository.GetRuleWhenUserSignInAuction();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(rule);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
