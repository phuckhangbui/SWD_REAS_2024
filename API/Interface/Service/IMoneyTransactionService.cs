using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interface.Service
{
    public interface IMoneyTransactionService
    {
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionParam moneyTransactionParam);
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId);
        Task<MoneyTransaction> CreateMoneyTransactionFromDepositPayment(DepositPaymentDto paymentDto);
    }
}
