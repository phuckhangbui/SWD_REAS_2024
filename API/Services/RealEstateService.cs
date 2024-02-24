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
    public class RealEstateService : BaseService<RealEstate>, IRealEstateService
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        public RealEstateService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _real_estate_detail_repository = realEstateDetailRepository;
            _real_estate_repository = realEstateRepository;
        }

        public async Task<PageList<RealEstateDto>> ListRealEstate()
        {
            var reals = await _real_estate_repository.GetAllRealEstateOnRealEstatePage();
            return reals;
        }

        public async Task<PageList<RealEstateDto>> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto)
        {
            var reals = await _real_estate_repository.SearchRealEstateByKey(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> ViewRealEstateDetail(int id)
        {
            var _real_estate_detail = await _real_estate_detail_repository.GetRealEstateDetail(id);
            return _real_estate_detail;
        }
    }
}
