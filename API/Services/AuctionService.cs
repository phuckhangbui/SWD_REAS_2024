using API.DTOs;
using API.Entity;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using API.Repository;
using AutoMapper;

namespace API.Services
{
    public class AuctionService : BaseService<Auction>, IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAccountRepository _accountRepository;

        public AuctionService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _auctionRepository = auctionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<PageList<AuctionDto>> GetAuctionHisotoryForAttender(AuctionHistoryParam auctionAccountingParam)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(auctionAccountingParam.AccountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {auctionAccountingParam.AccountId} not found.");
            }

            return await _auctionRepository.GetAuctionHistoryForAttenderAsync(auctionAccountingParam);
        }

        public async Task<PageList<AuctionDto>> GetAuctionHisotoryForOwner(AuctionHistoryParam auctionAccountingParam)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(auctionAccountingParam.AccountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {auctionAccountingParam.AccountId} not found.");
            }

            return await _auctionRepository.GetAuctionHistoryForOwnerAsync(auctionAccountingParam);
        }

        public async Task<PageList<AuctionDto>> GetAuctions(AuctionParam auctionParam)
        {
            var auctions = await _auctionRepository.GetAuctions(auctionParam);
            return auctions;
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
