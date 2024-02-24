using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MoneyTransactionRepository : BaseRepository<MoneyTransaction>, IMoneyTransactionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MoneyTransactionRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionParam moneyTransactionParam)
        {
            var query = _context.MoneyTransaction.AsQueryable();

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
