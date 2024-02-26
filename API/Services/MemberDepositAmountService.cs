using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using AutoMapper;

namespace API.Services
{
    public class MemberDepositAmountService : BaseService<DepositAmount>, IMemberDepositAmountService
    {
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IAccountRepository _accountRepository;
        public MemberDepositAmountService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _accountRepository = accountRepository;
            _depositAmountRepository = depositAmountRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<PageList<DepositAmountDto>> ListDepositAmoutByMember(int userMember)
        {
            var deposit = await _depositAmountRepository.GetDepositAmoutForMember(userMember);
            return deposit;
        }

        public async Task<PageList<DepositAmountDto>> ListDepositAmoutByMemberWhenSearch(SearchDepositAmountParam searchDepositAmountParam, int userMember)
        {
            var deposit = await _depositAmountRepository.GetDepositAmoutForMemberBySearch(searchDepositAmountParam, userMember);
            return deposit;
        }
    }
}
