using API.Entity;
using API.DTOs;

namespace API.Interfaces
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idAccount);
        Task<int> GetIdTransactionWhenCreateNewTransaction();
    }
}
