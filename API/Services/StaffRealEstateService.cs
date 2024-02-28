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
    public class StaffRealEstateService : BaseService<RealEstate>, IStaffRealEstateService
    {
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;

        public StaffRealEstateService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _realEstateRepository = realEstateRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingByStaff()
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoing();
            return reals;
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateExceptOnGoingByStaffBySearch(SearchRealEstateParam searchRealEstateDto)
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoingBySearch(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateExceptOnGoingDetailByStaff(int id)
        {
            var real_estate_detail = await _realEstateDetailRepository.GetRealEstateDetail(id);
            return real_estate_detail;
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoingByStaff()
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoing();
            return reals;
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoingByStaffBySearch(SearchRealEstateParam searchRealEstateDto)
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoingBySearch(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateOnGoingDetailByStaff(int id)
        {
            var real_estate_detail = await _realEstateDetailRepository.GetRealEstateDetail(id);
            return real_estate_detail;
        }

        public async Task<bool> UpdateStatusRealEstateByStaff(ReasStatusParam reasStatusDto)
        {
            try
            {
                bool updateReal = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusDto);
                if (updateReal) { return true; }
                else { return false; }
            }
            catch { return false; }
        }
    }
}
