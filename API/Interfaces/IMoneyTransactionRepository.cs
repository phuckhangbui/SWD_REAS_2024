using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idAccount);
        Task<int> GetIdTransactionWhenCreateNewTransaction();
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionParam moneyTransactionParam);
    }
}
