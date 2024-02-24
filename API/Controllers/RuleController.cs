using API.Entity;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly IRuleService _ruleService;
        private const string BaseUri = "/api/admin/";
        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpGet(BaseUri + "rule")]
        public async Task<ActionResult<Rule>> GetAllRule([FromQuery] PaginationParams paginationParams)
        {
            int idAdmin = GetIdAdmin(_ruleService.AccountRepository);
            if (idAdmin != 0)
            {
                var rule = await _ruleService.GetAllRule();
                Response.AddPaginationHeader(new PaginationHeader(rule.CurrentPage, rule.PageSize,
                rule.TotalCount, rule.TotalPages));
                if (rule.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(rule);
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "rule/add")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRule(RuleCreateParam ruleCreate)
        {
            int idAdmin = GetIdAdmin(_ruleService.AccountRepository);
            if (idAdmin != 0)
            {
                var rule = await _ruleService.CreateNewRule(ruleCreate);
                if (rule)
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

        [HttpPost(BaseUri + "rule/update")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateRule(RuleChangeContentParam ruleChangeContent)
        {
            int idAdmin = GetIdAdmin(_ruleService.AccountRepository);
            if (idAdmin != 0)
            {
                var rule = await _ruleService.UpdateRule(ruleChangeContent);
                if (rule)
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
