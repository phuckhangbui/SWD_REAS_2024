using API.Entity;
using API.Param;

namespace API.Interface.Repository
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idAccount);
        Task<int> GetIdTransactionWhenCreateNewTransaction();
    }
}
