using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using API.Param.Enums;
using AutoMapper;

namespace API.Services
{
    public class AdminRealEstateService : BaseService<RealEstate>, IAdminRealEstateService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        public AdminRealEstateService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _accountRepository = accountRepository;
            _realEstateRepository = realEstateRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<bool> BlockRealEstate(int reasId)
        {
            ReasStatusParam reasStatus = new ReasStatusParam();
            reasStatus.Id = reasId;
            reasStatus.status = (int)RealEstateEnum.Block;
            reasStatus.statusMessage = "";

            bool check = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (check)
            {
                return true;
            }
            else { return false; }
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingByAdmin()
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoing();
            return reals;
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstatesBySearch(SearchRealEstateParam searchRealEstateParam)
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoingBySearch(searchRealEstateParam);
            return reals;
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstatesPendingBySearch(SearchRealEstateParam searchRealEstateParam)
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoingBySearch(searchRealEstateParam);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateAllDetail(int reasId)
        {
            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetail(reasId);
            return realEstateDetailDto;
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoingByAdmin()
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoing();
            return reals;
        }

        public  async Task<RealEstateDetailDto> GetRealEstatePendingDetail(int reasId)
        {
            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetail(reasId);
            return realEstateDetailDto;
        }

        public async Task<bool> UnblockRealEstate(int reasId)
        {
            ReasStatusParam reasStatus = new ReasStatusParam();
            reasStatus.Id = reasId;
            reasStatus.status = (int)RealEstateEnum.Selling;
            reasStatus.statusMessage = "";

            bool check = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (check)
            {
                return true;
            }
            else { return false; }
        }

        public async Task<bool> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusParam)
        {
            bool updateReal = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusParam);
            if (updateReal)
            {
                return true;
            }
            else { return false; }
        }
    }
}
