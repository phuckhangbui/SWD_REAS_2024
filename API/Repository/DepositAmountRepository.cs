using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Validate;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class DepositAmountRepository : BaseRepository<DepositAmount>, IDepositAmountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DepositAmountRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmountsAsync(DepositAmountParam depositAmountParam)
        {
            var query = _context.DepositAmount.AsQueryable();

            if (!string.IsNullOrEmpty(depositAmountParam.AmountFrom) && !string.IsNullOrEmpty(depositAmountParam.AmountTo))
            {
                if (int.TryParse(depositAmountParam.AmountFrom, out var amountFrom) &&
                    int.TryParse(depositAmountParam.AmountTo, out var amountTo))
                {
                    query = query.Where(a => string.Compare(a.Amount, depositAmountParam.AmountFrom) >= 0 &&
                                             string.Compare(a.Amount, depositAmountParam.AmountTo) <= 0);
                }
            }

            if (depositAmountParam.DepositDateFrom != default(DateTime) && depositAmountParam.DepositDateTo != default(DateTime))
            {
                query = query.Where(a => a.DepositDate >= depositAmountParam.DepositDateFrom
                                        && a.DepositDate <= depositAmountParam.DepositDateTo);
            }

            if (depositAmountParam.Status != -1)
            {
                query = query.Where(a => a.Status == depositAmountParam.Status);
            }

            query = query.OrderByDescending(q => q.DepositDate);

            return await PageList<DepositAmountDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
                depositAmountParam.PageNumber,
                depositAmountParam.PageSize);
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmoutForMember(int id)
        {
            var getNameStaus = new GetStatusName();
            PaginationParams paginationParams = new PaginationParams();
            var depositAmountByAccount = _context.DepositAmount.Where(x => x.AccountSignId == id).Select(x => new DepositAmountDto
            {
                DepositId = x.DepositId,
                Amount = x.Amount,
                AccountSignId = x.AccountSignId,
                DepositDate = x.DepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = getNameStaus.GetDepositAmountStatusName(x.Status),
            });
            depositAmountByAccount = depositAmountByAccount.OrderByDescending(x => x.DepositDate);
            return await PageList<DepositAmountDto>.CreateAsync(
            depositAmountByAccount.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
            paginationParams.PageNumber,
            paginationParams.PageSize);
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmoutForMemberBySearch(SearchDepositAmountParam searchDepositAmountDto, int id)
        {
            var getNameStaus = new GetStatusName();
            var parseValidate = new ParseValidate();
            PaginationParams paginationParams = new PaginationParams();
            var depositAmountBySearch = _context.DepositAmount.Where(x => x.AccountSignId == id &&
            ((string.IsNullOrEmpty(searchDepositAmountDto.AmountFrom) && string.IsNullOrEmpty(searchDepositAmountDto.AmountTo)) ||
            ((parseValidate.ParseStringToInt(x.Amount) >= parseValidate.ParseStringToInt(searchDepositAmountDto.AmountFrom)) &&
            (parseValidate.ParseStringToInt(x.Amount) <= parseValidate.ParseStringToInt(searchDepositAmountDto.AmountTo))) &&
            ((searchDepositAmountDto.DepositDateFrom == null && searchDepositAmountDto.DepositDateTo == null) ||
            (x.DepositDate >= searchDepositAmountDto.DepositDateFrom &&
            x.DepositDate <= searchDepositAmountDto.DepositDateTo))))
            .Select(x => new DepositAmountDto
            {
                DepositId = x.DepositId,
                Amount = x.Amount,
                AccountSignId = x.AccountSignId,
                DepositDate = x.DepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = getNameStaus.GetDepositAmountStatusName(x.Status),
            });

            depositAmountBySearch = depositAmountBySearch.OrderByDescending(x => x.DepositDate);
            return await PageList<DepositAmountDto>.CreateAsync(
            depositAmountBySearch.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
            paginationParams.PageNumber,
            paginationParams.PageSize);
        }


        public DepositAmount GetDepositAmount(int accountSignId, int reasId) => _context.DepositAmount.FirstOrDefault(d => d.AccountSignId == accountSignId && d.ReasId == reasId);
    }
}
