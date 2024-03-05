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
    public class DepositAmountRepository : BaseRepository<DepositAmount>, IDepositAmountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DepositAmountRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageList<DepositDto>> GetDepositAmountsAsync(DepositAmountParam depositAmountParam)
        {
            var query = _context.DepositAmount.AsQueryable();
            query = query
                .Include(a => a.RealEstate)
                .Include(a => a.AccountSign);

            if (depositAmountParam.AmountTo != 0 && depositAmountParam.AmountFrom != 0)
            {
                query = query.Where(a => a.Amount >= depositAmountParam.AmountFrom && a.Amount <= depositAmountParam.AmountTo);
            }

            if (depositAmountParam.DepositDateFrom != default && depositAmountParam.DepositDateTo != default)
            {
                query = query.Where(a => a.DepositDate >= depositAmountParam.DepositDateFrom
                                        && a.DepositDate <= depositAmountParam.DepositDateTo);
            }

            if (depositAmountParam.Status != -1)
            {
                query = query.Where(a => a.Status == depositAmountParam.Status);
            }

            query = query.OrderByDescending(q => q.DepositDate);

            return await PageList<DepositDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<DepositDto>(_mapper.ConfigurationProvider),
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
                DepositDate = (DateTime)x.DepositDate,
                CreateDepositDate = x.CreateDepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = x.Status,
                DisplayStatus = getNameStaus.GetDepositAmountStatusName(x.Status),
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
            PaginationParams paginationParams = new PaginationParams();
            var depositAmountBySearch = _context.DepositAmount.Where(x => x.AccountSignId == id &&
            ((searchDepositAmountDto.AmountFrom == 0 && searchDepositAmountDto.AmountTo == 0) ||
            ((x.Amount >= searchDepositAmountDto.AmountFrom) &&
            (x.Amount <= searchDepositAmountDto.AmountTo)) &&
            ((searchDepositAmountDto.DepositDateFrom == null && searchDepositAmountDto.DepositDateTo == null) ||
            (x.DepositDate >= searchDepositAmountDto.DepositDateFrom &&
            x.DepositDate <= searchDepositAmountDto.DepositDateTo))))
            .Select(x => new DepositAmountDto
            {
                DepositId = x.DepositId,
                Amount = x.Amount,
                AccountSignId = x.AccountSignId,
                DepositDate = (DateTime)x.DepositDate,
                CreateDepositDate = x.CreateDepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = x.Status,
                DisplayStatus = getNameStaus.GetDepositAmountStatusName(x.Status),
            });

            depositAmountBySearch = depositAmountBySearch.OrderByDescending(x => x.DepositDate);
            return await PageList<DepositAmountDto>.CreateAsync(
            depositAmountBySearch.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
            paginationParams.PageNumber,
            paginationParams.PageSize);
        }


        public List<DepositAmount> GetDepositAmounts(int accountSignId, int reasId) => _context.DepositAmount.Where(d => d.AccountSignId == accountSignId && d.ReasId == reasId).ToList();

        public DepositAmount GetDepositAmount(int accountSignId, int reasId)
        {
            return _context.DepositAmount
                .FirstOrDefault(d => d.AccountSignId == accountSignId && d.ReasId == reasId);
        }


        public DepositAmount GetDepositAmount(int depositId)
        {
            return _context.DepositAmount.FirstOrDefault(d => d.DepositId == depositId);
        }

        public DepositDetailDto GetDepositDetailAsync(int depositId)
        {
            var depositDetail = _context.DepositAmount
                .Include(d => d.RealEstate)
                .Include(d => d.AccountSign)
                .FirstOrDefault(d => d.DepositId == depositId);

            return _mapper.Map<DepositDetailDto>(depositDetail);
        }

        public async Task<PageList<AccountMemberDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId)
        {
            var query = _context.Account
                .Join(_context.DepositAmount,
                    account => account.AccountId,
                    depositAmount => depositAmount.AccountSignId,
                    (account, depositAmount) => new { Account = account, DepositAmount = depositAmount })
                .Join(_context.RealEstate,
                    joinResult => joinResult.DepositAmount.ReasId,
                    realEstate => realEstate.ReasId,
                    (joinResult, realEstate) => new { Account = joinResult.Account, DepositAmount = joinResult.DepositAmount, RealEstate = realEstate })
                .Where(joinResult => joinResult.DepositAmount.ReasId == reasId &&
                                     joinResult.DepositAmount.Status == (int)UserDepositEnum.Deposited &&
                                     joinResult.RealEstate.ReasStatus == (int)RealEstateEnum.Selling)
                .Select(joinResult => new AccountMemberDto
                {
                    AccountId = joinResult.Account.AccountId,
                    AccountName = joinResult.Account.AccountName,
                    AccountEmail = joinResult.Account.AccountEmail,
                    Account_Status = joinResult.Account.Account_Status.ToString(),
                    Date_Created = joinResult.Account.Date_Created
                });

            return await PageList<AccountMemberDto>.CreateAsync(
                query.AsNoTracking(),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }
    }
}
