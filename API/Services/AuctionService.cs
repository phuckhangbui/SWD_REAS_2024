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
    public class AuctionService : BaseService<Auction>, IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<bool> CreateAuction(AuctionCreateParam auctionCreateParam)
        {
            bool check = await _auctionRepository.CreateAuction(auctionCreateParam);
            if (check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<DepositAmountUserDto>> GetAllUserForDeposit(int id)
        {
            var deposit = await _auctionRepository.GetAllUserForDeposit(id);
            return deposit;
        }

        public async Task<AuctionDetailFinish> GetAuctionDetailFinish(int id)
        {
            var auctiondetail = await _auctionRepository.GetAuctionDetailFinish(id);
            return auctiondetail;
        }

        public async Task<AuctionDetailOnGoing> GetAuctionDetailOnGoing(int id)
        {
            var auctiondetail = await _auctionRepository.GetAuctionDetailOnGoing(id);
            return auctiondetail;
        }


        public async Task<IEnumerable<AuctionDto>> GetAuctionsFinish()
        {
            var auctions = await _auctionRepository.GetAuctionsFinish();
            return auctions;
        }

        public async Task<IEnumerable<AuctionDto>> GetAuctionsNotYetAndOnGoing()
        {
            var auctions = await _auctionRepository.GetAuctionsNotYetAndOnGoing();
            return auctions;
        }

        public async Task<IEnumerable<ReasForAuctionDto>> GetAuctionsReasForCreate()
        {
            var real = await _auctionRepository.GetAuctionsReasForCreate();
            return real;
        }

        public async Task<PageList<AuctionDto>> GetRealEstates(AuctionParam auctionParam)
        {
            var auctions = await _auctionRepository.GetAuctionsAsync(auctionParam);
            return auctions;
        }

        public async Task<bool> ToggleAuctionStatus(string auctionId, string statusCode)
        {
            try
            {
                bool check = await _auctionRepository.EditAuctionStatus(auctionId, statusCode);
                if (check) { return true; }
                else return false;
            }catch (Exception ex) { return  false; }
        }
    }
}
