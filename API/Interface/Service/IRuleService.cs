using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;


namespace API.Interface.Service
{
    public interface IRuleService : IBaseService<Rule>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<Rule>> GetAllRule();
        Task<bool> CreateNewRule(RuleCreateParam ruleCreate);
        Task<bool> UpdateRule(RuleChangeContentParam ruleChangeContent);
    }
}
