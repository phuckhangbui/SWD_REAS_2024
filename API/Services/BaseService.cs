using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
       
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IRealEstatePhotoRepository _realEstatePhotoRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IMoneyTransactionDetailRepository _moneyTransactionDetailRepository;
        private readonly IMoneyTransactionRepository _moneyTransactionRepository;
        private readonly IRuleRepository _ruleRepository;
        private readonly ITypeReasRepository _typeReasRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly ITokenService _tokenService;

        public BaseService(IAccountRepository accountRepository,
            IRealEstateRepository realEstateRepository,
            IRealEstateDetailRepository realEstateDetailRepository,
            IRealEstatePhotoRepository realEstatePhotoRepository,
            INewsRepository newsRepository,
            IMoneyTransactionRepository moneyTransactionRepository,
            IMoneyTransactionDetailRepository moneyTransactionDetailRepository,
            IRuleRepository ruleRepository,
            ITypeReasRepository typeReasRepository,
            IAuctionRepository auctionRepository,
            IDepositAmountRepository depositAmountRepository,
            IMapper mapper,
            IPhotoService photoService,
            ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _auctionRepository = auctionRepository;
            _moneyTransactionRepository = moneyTransactionRepository;
            _moneyTransactionDetailRepository = moneyTransactionDetailRepository;
            _ruleRepository = ruleRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _realEstatePhotoRepository = realEstatePhotoRepository;
            _realEstateRepository = realEstateRepository;
            _typeReasRepository = typeReasRepository;
            _depositAmountRepository = depositAmountRepository;
            _mapper = mapper;
            _photoService = photoService;
            _tokenService = tokenService;
        }

    }
}
