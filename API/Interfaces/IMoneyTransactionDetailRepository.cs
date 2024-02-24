using API.DTOs;
using API.Entity;

namespace API.Interfaces
{
    public interface IMoneyTransactionDetailRepository : IBaseRepository<MoneyTransactionDetail>
    {
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId);
    }
}
