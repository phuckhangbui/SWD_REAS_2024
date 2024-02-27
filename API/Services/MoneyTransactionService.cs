using API.DTOs;
using API.Entity;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;
using API.Param.Enums;

namespace API.Services
{
    public class MoneyTransactionService : IMoneyTransactionService
    {
        private readonly IMoneyTransactionRepository _moneyTransactionRepository;
        private readonly IMoneyTransactionDetailRepository _moneyTransactionDetailRepository;

        public MoneyTransactionService(IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository)
        {
            _moneyTransactionRepository = moneyTransactionRepository;
            _moneyTransactionDetailRepository = moneyTransactionDetailRepository;
        }

        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId)
        {
            var transactionDetail = await _moneyTransactionDetailRepository.GetMoneyTransactionDetailAsync(transactionId);

            if (transactionDetail == null)
            {
                throw new BaseNotFoundException($"Transaction detail with ID {transactionId} not found.");
            }

            return transactionDetail;
        }

        public Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionParam moneyTransactionParam)
        {
            return _moneyTransactionRepository.GetMoneyTransactionsAsync(moneyTransactionParam);
        }

        public async Task<MoneyTransaction> CreateMoneyTransactionFromDepositPayment(DepositPaymentDto paymentDto)
        {
            MoneyTransaction moneyTransaction = new MoneyTransaction();

            moneyTransaction.TypeId = 1;
            moneyTransaction.AccountSendId = paymentDto.CustomerId;
            moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
            moneyTransaction.Money = paymentDto.Money.ToString();
            moneyTransaction.DateExecution = paymentDto.PaymentTime;


            MoneyTransactionDetail moneyTransactionDetail = new MoneyTransactionDetail();
            moneyTransactionDetail.ReasId = paymentDto.ReasId;
            moneyTransactionDetail.TotalAmmount = paymentDto.Money.ToString();
            moneyTransactionDetail.PaidAmount = paymentDto.Money.ToString();
            moneyTransactionDetail.RemainingAmount = "0";
            moneyTransactionDetail.DateExecution = paymentDto.PaymentTime;
            moneyTransactionDetail.AccountReceiveId = null;
            moneyTransactionDetail.AuctionId = null;
            //moneyTransactionDetail.MoneyTransactionDetailId = null;

            await _moneyTransactionRepository.CreateMoneyTransactionAndMoneyTransactionDetail(moneyTransaction, moneyTransactionDetail);

            return moneyTransaction;
        }
    }
}
