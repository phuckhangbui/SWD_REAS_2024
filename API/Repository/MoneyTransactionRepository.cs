using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Interfaces;
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

        public async Task<TransactionMoneyCreateDto> CreateNewMoneyTransaction(TransactionMoneyCreateDto transactionMoneyCreateDto, int idAccount)
        {
            MoneyTransaction moneyTransaction = new MoneyTransaction();
            moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
            moneyTransaction.TypeId = 3;
            moneyTransaction.DateExecution = DateTime.UtcNow;
            moneyTransaction.AccountSendId = idAccount;
            moneyTransaction.Money = transactionMoneyCreateDto.MoneyPaid;
            try
            {
                await CreateAsync(moneyTransaction);
                return transactionMoneyCreateDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetIdTransactionWhenCreateNewTransaction()
        {
            return await _dataContext.MoneyTransaction.MaxAsync(x => x.TransactionId);
        }
    }
}
