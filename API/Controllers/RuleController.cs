using API.Data;
using API.Entity;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly RuleRepository _rule_repository;
        public RuleController(DataContext dataContext, RuleRepository ruleRepository)
        {
            _dataContext = dataContext;
            _rule_repository = ruleRepository;
        }

        [HttpGet("/home/real_estate/rule")]
        public async Task<ActionResult<Rule>> GetRule()
        {
            var rule = _rule_repository.GetAll().Where(x => x.Title == "Rule").Select(x => new Rule
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
