using API.Data;
using API.DTOs;
using API.Entity;
using API.Errors;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;
        public RuleController(IRuleRepository ruleRepository, IAccountRepository account_repository)
        {
            _rule_repository = ruleRepository;
            _account_repository = account_repository;
        }

        [HttpGet("/home/real_estate/rule")]
        public async Task<ActionResult<Rule>> GetRuleContractWhenUserSignInAuction()
        {
            int userMember = GetLoginAccountId();
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

        [HttpGet("/admin/rule")]
        public async Task<ActionResult<List<Rule>>> GetAllRule([FromQuery] PaginationParams paginationParams)
        {
            int idAdmin = GetIdAdmin(_account_repository);
            if (idAdmin != 0)
            {
                var rule = _rule_repository.GetAllRule();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(rule);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("/admin/rule/add")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRule(RuleCreateDto ruleCreate)
        {
            int idAdmin = GetIdAdmin(_account_repository);
            if (idAdmin != 0)
            {
                var rule = _rule_repository.CreateNewRule(ruleCreate);
                if (rule != null)
                {
                    return new ApiResponseMessage("MSG18");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("/admin/rule/update")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateRule(RuleChangeContentDto ruleChangeContent)
        {
            int idAdmin = GetIdAdmin(_account_repository);
            if (idAdmin != 0)
            {
                var rule = _rule_repository.UpdateRuleByContentChange(ruleChangeContent);
                if (rule != null)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
