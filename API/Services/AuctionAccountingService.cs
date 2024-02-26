using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.ThirdServices;

namespace API.Services
{
    public class AuctionAccountingService : IAuctionAccountingService
    {
        public readonly IAuctionAccountingRepository _auctionAccountingRepository;
        public readonly IAuctionRepository _auctionRepository;
        public readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;

        readonly float DEPOSIT_PERCENT = 0.1f;
        readonly float COMMISSION_PERCENT = 0.02f;
        readonly int DATE_UNTIL_PAY = 3;

        public AuctionAccountingService(IAuctionAccountingRepository auctionAccountingRepository, IAuctionRepository auctionRepository, IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository)
        {
            _auctionAccountingRepository = auctionAccountingRepository;
            _auctionRepository = auctionRepository;
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
        }

        public async System.Threading.Tasks.Task<AuctionAccounting> UpdateAuctionAccounting(AuctionDetailDto auctionDetailDto)
        {
            //get auction accounting
            AuctionAccounting auctionAccountingOld = _auctionAccountingRepository.GetAuctionAccountingByAuctionId(auctionDetailDto.AuctionId);

            if (auctionAccountingOld != null)
            {
                ICollection<AuctionAccounting> list = new List<AuctionAccounting>();
                list.Add(auctionAccountingOld);
                await _auctionAccountingRepository.DeleteAsync(list);
            }

            //create new auction accounting base on input
            AuctionAccounting auctionAccounting = new AuctionAccounting();
            Auction auction = _auctionRepository.GetAuction(auctionDetailDto.AuctionId);
            var realEstate = await _realEstateDetailRepository.GetRealEstateDetail(auction.ReasId);
            Account accountWin = await _accountRepository.GetAccountByAccountIdAsync(auctionDetailDto.AccountWinId);

            auctionAccounting.AuctionId = auctionDetailDto.AuctionId;
            auctionAccounting.ReasId = auction.ReasId;
            auctionAccounting.AccountWinId = auctionDetailDto.AccountWinId;
            auctionAccounting.AccountWinName = accountWin.AccountName;
            auctionAccounting.AccountOwnerId = realEstate.AccountOwnerId;
            auctionAccounting.AccountOwnerName = realEstate.AccountOwnerName;
            auctionAccounting.EstimatedPaymentDate = DateTime.Now.AddDays(DATE_UNTIL_PAY);

            auctionAccounting.MaxAmount = auctionDetailDto.WinAmount;
            auctionAccounting.DepositAmount = float.Parse(realEstate.ReasPrice) * DEPOSIT_PERCENT;
            auctionAccounting.CommissionAmount = auctionDetailDto.WinAmount * COMMISSION_PERCENT;
            auctionAccounting.AmountOwnerReceived = auctionDetailDto.WinAmount - auctionAccounting.CommissionAmount;

            try
            {
                await _auctionAccountingRepository.CreateAsync(auctionAccounting);
            }
            catch (Exception ex)
            {
                return null;
            }

            return auctionAccounting;

        }

        public async System.Threading.Tasks.Task SendWinnerEmail(AuctionAccounting auctionAccounting)
        {
            Auction auction = _auctionRepository.GetAuction(auctionAccounting.AuctionId);
            var realEstate = await _realEstateDetailRepository.GetRealEstateDetail(auction.ReasId);
            Account accountWin = await _accountRepository.GetAccountByAccountIdAsync(auctionAccounting.AccountWinId);


            SendMailAuctionSuccess.SendMailWhenAuctionSuccess(accountWin.AccountEmail, realEstate.ReasName, realEstate.ReasAddress, DateOnly.FromDateTime(auctionAccounting.EstimatedPaymentDate), auctionAccounting.MaxAmount, auctionAccounting.DepositAmount);
        }
    }
}
