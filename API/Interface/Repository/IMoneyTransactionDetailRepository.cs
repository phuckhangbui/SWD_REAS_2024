using API.Entity;
using API.Param;

namespace API.Interface.Repository
{
    public interface IMoneyTransactionDetailRepository : IBaseRepository<MoneyTransactionDetail>
    {
        Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idTransaction);
    }
}
