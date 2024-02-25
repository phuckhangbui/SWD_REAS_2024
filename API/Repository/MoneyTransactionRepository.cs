using API.Data;
using API.Entity;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MoneyTransactionRepository : BaseRepository<MoneyTransaction>, IMoneyTransactionRepository
    {
        private readonly DataContext _dataContext;
        public MoneyTransactionRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idAccount)
        {
            MoneyTransaction moneyTransaction = new MoneyTransaction();
            moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
            moneyTransaction.TypeId = 3;
            moneyTransaction.DateExecution = DateTime.UtcNow;
            moneyTransaction.AccountSendId = idAccount;
            moneyTransaction.Money = transactionMoneyCreateDto.MoneyPaid;
            try
            {
                bool check = await CreateAsync(moneyTransaction);
                if (check)
                {
                    return true; 
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> GetIdTransactionWhenCreateNewTransaction()
        {
            return await _dataContext.MoneyTransaction.MaxAsync(x => x.TransactionId);
        }
    }
}
