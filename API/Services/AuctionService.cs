using API.DTOs;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;


namespace API.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAccountRepository _accountRepository;

        public AuctionService(IAuctionRepository auctionRepository, IAccountRepository accountRepository)
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
            }
            catch (Exception ex) { return false; }
        }
    }
}
