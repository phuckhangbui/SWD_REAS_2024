using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionParam moneyTransactionParam);
    }
}
