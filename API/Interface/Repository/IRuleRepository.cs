using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface IRuleRepository : IBaseRepository<Rule>
    {
        Task<Rule> GetRuleWhenUserSignInAuction();
        Task<bool> CreateNewRule(RuleCreateParam ruleCreate);
        Task<PageList<Rule>> GetAllRule();
        Task<bool> UpdateRuleByContentChange(RuleChangeContentParam ruleChangeContent);
    }
}
