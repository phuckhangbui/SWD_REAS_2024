using API.Data;
using API.Entity;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly RuleRepository _rule_repository;
        public RuleController(RuleRepository ruleRepository) : base(ruleRepository)
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
    }
}
