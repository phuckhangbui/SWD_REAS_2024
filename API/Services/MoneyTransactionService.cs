using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class MoneyTransactionService : IMoneyTransactionService
    {
        private readonly IMoneyTransactionRepository _moneyTransactionRepository;

        public MoneyTransactionService(IMoneyTransactionRepository moneyTransactionRepository)
        {
            _moneyTransactionRepository = moneyTransactionRepository;
        }

        //public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId)
        //{
        //    //var transactionDetail = await _moneyTransactionDetailRepository.GetMoneyTransactionDetailAsync(transactionId);
        //    var transactionDetail;
        //    if (transactionDetail == null)
        //    {
        //        throw new BaseNotFoundException($"Transaction detail with ID {transactionId} not found.");
        //    }

        //    return transactionDetail;
        //}

        public Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionParam moneyTransactionParam)
        {
            return _moneyTransactionRepository.GetMoneyTransactionsAsync(moneyTransactionParam);
        }

        public async System.Threading.Tasks.Task<bool> CreateMoneyTransaction(MoneyTransaction moneyTransaction)
        {
            return await _moneyTransactionRepository.CreateAsync(moneyTransaction);
        }

        //public async Task<MoneyTransaction> CreateMoneyTransactionFromDepositPayment(DepositPaymentDto paymentDto)
        //{
        //    MoneyTransaction moneyTransaction = new MoneyTransaction();

        //    moneyTransaction.TypeId = 1;
        //    moneyTransaction.AccountSendId = paymentDto.CustomerId;
        //    moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
        //    moneyTransaction.Money = paymentDto.Money;
        //    moneyTransaction.DateExecution = paymentDto.PaymentTime;


        //    //MoneyTransactionDetail moneyTransactionDetail = new MoneyTransactionDetail();
        //    //moneyTransactionDetail.ReasId = paymentDto.ReasId;
        //    //moneyTransactionDetail.TotalAmmount = paymentDto.Money;
        //    //moneyTransactionDetail.PaidAmount = paymentDto.Money;
        //    //moneyTransactionDetail.RemainingAmount = 0;
        //    //moneyTransactionDetail.DateExecution = paymentDto.PaymentTime;
        //    //moneyTransactionDetail.AccountReceiveId = null;
        //    //moneyTransactionDetail.AuctionId = null;
        //    ////moneyTransactionDetail.MoneyTransactionDetailId = null;

        //    //await _moneyTransactionRepository.CreateMoneyTransactionAndMoneyTransactionDetail(moneyTransaction, moneyTransactionDetail);

        //    return moneyTransaction;
        //}
    }
}
