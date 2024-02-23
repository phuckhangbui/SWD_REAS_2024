using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly IRuleRepository _rule_repository;
        public RuleController(IRuleRepository ruleRepository)
        {
            _rule_repository = ruleRepository;
        }

        [HttpGet("/home/real_estate/rule")]
        public async Task<ActionResult<Rule>> GetRule()
        {
            var rule = _rule_repository.GetAllAsync().Result.Where(x => x.Title == "Rule").Select(x => new Rule
            {
                Title = x.Title,
                Content = x.Content,
            }).FirstOrDefault();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rule);
        }

        [HttpGet("/admin/rule")]
        public async Task<ActionResult<List<Rule>>> GetAllRule()
        {
            var rule = _rule_repository.GetAllAsync();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rule);
        }

        [HttpPost("CreateRule")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRule(RuleCreateDto ruleCreate)
        {
            var rule = new Rule
            {
                Title = ruleCreate.Title,
                Content = ruleCreate.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
            };
            _rule_repository.CreateAsync(rule).Wait();
            return new ApiResponseMessage("MSG");
        }

        [HttpPost("UpdateRule")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateRule (RuleCreateDto ruleUpdate)
        {
            var rule = new Rule
            {
                Title = ruleUpdate.Title,
                Content = ruleUpdate.Content,
                DateCreated = ruleUpdate.DateCreated,
                DateUpdated = DateTime.UtcNow,
            };
            _rule_repository.UpdateAsync(rule).Wait();
            return new ApiResponseMessage("MSG03");
        }
    }
}
