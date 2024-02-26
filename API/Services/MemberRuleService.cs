using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Repository;
using AutoMapper;

namespace API.Services
{
    public class MemberRuleService : BaseService<Rule>, IMemberRuleService
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;

        public MemberRuleService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _rule_repository = ruleRepository;
            _account_repository = accountRepository;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<Rule> GetRuleContractWhenUserSignInAuction()
        {
            var rule = await _rule_repository.GetRuleWhenUserSignInAuction();
            return rule;
        }
    }
}
