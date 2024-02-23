using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IRuleRepository : IBaseRepository<Rule>
    {
        Task<Rule> GetRuleWhenUserSignInAuction();
        Task<Rule> CreateNewRule(RuleCreateDto ruleCreate);
        Task<PageList<Rule>> GetAllRule();
        Task<RuleChangeContentDto> UpdateRuleByContentChange(RuleChangeContentDto ruleChangeContent);
    }
}
