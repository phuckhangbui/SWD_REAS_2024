using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MoneyTransactionRepository : BaseRepository<MoneyTransaction>, IMoneyTransactionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public MoneyTransactionRepository(DataContext context, IMapper mapper) : base(context)
        {
            _dataContext = context;
            _mapper = mapper;
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

        public async Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionParam moneyTransactionParam)
        {
            var query = _dataContext.MoneyTransaction.AsQueryable();

            if (moneyTransactionParam.TransactionStatus != 0)
            {
                query = query.Where(m => m.TransactionStatus == moneyTransactionParam.TransactionStatus);
            }

            if (moneyTransactionParam.TypeId != 0)
            {
                query = query.Where(m => m.TypeId == moneyTransactionParam.TypeId);
            }

            query = query.OrderByDescending(r => r.DateExecution);

            return await PageList<MoneyTransactionDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<MoneyTransactionDto>(_mapper.ConfigurationProvider),
            moneyTransactionParam.PageNumber,
            moneyTransactionParam.PageSize);
        }
    }
}
