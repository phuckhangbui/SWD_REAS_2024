using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using AutoMapper;

namespace API.Services
{
    public class RuleService : BaseService<Rule>, IRuleService
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;

        public RuleService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _rule_repository = ruleRepository;
            _account_repository = accountRepository;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<bool> CreateNewRule(RuleCreateParam ruleCreate)
        {
            try
            {
                bool rule = await _rule_repository.CreateNewRule(ruleCreate);
                if (rule) return true;
                else return false;
            }catch (Exception ex) { return false; }
        }

        public async Task<PageList<Rule>> GetAllRule()
        {
            var rule = await _rule_repository.GetAllRule();
            return rule;
        }

        public async Task<bool> UpdateRule(RuleChangeContentParam ruleChangeContent)
        {
            try
            {
                bool rule = await _rule_repository.UpdateRuleByContentChange(ruleChangeContent);
                if (rule) return true;
                else return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
