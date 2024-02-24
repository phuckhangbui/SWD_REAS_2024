using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MoneyTransactionDetailRepository : BaseRepository<MoneyTransactionDetail>, IMoneyTransactionDetailRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MoneyTransactionDetailRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId)
        {
            var transactionDetail = await _context.MoneyTransactionDetail.SingleOrDefaultAsync(m => m.MoneyTransactionId == transactionId);
            if (transactionDetail != null)
            {
                return _mapper.Map<MoneyTransactionDetailDto>(transactionDetail);
            }

            return null;
        }
    }
}
