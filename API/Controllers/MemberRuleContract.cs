using API.Entity;
using API.Errors;
using API.Interface.Repository;
using API.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberRuleContract : BaseApiController
    {
        private readonly IMemberRuleService _memberRuleService;
        private const string BaseUri = "/api/home/"; 

        public MemberRuleContract(IMemberRuleService memberRuleService)
        {
            _memberRuleService = memberRuleService;
        }

        [HttpGet(BaseUri + "real_estate/rule")]
        public async Task<ActionResult<Rule>> GetRuleContractWhenUserSignInAuction()
        {
            int userMember = GetIdMember(_memberRuleService.AccountRepository);
            if (userMember != 0)
            {
                var rule = _memberRuleService.GetRuleContractWhenUserSignInAuction();
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
