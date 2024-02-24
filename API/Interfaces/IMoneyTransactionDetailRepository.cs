using API.DTOs;
using API.Entity;

namespace API.Interfaces
{
    public interface IMoneyTransactionDetailRepository : IBaseRepository<MoneyTransactionDetail>
    {
        Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idTransaction);
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId);
    }
}
