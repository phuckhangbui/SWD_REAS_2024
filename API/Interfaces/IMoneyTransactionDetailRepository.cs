using API.Entity;
using API.DTOs;

namespace API.Interfaces
{
    public interface IMoneyTransactionDetailRepository : IBaseRepository<MoneyTransactionDetail>
    {
        Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idTransaction);
    }
}
