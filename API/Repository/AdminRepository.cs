using API.DTOs;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AdminRepository : IAdminRepository
{
    private readonly DataContext context;

    private readonly IMapper mapper;

    public AdminRepository(DataContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }


    public async Task<PageList<AccountDto>> GetAccounts(AccountParams accountParams)
    {
        var query = context.Account.AsQueryable();

        query = query.Where(a => a.RoleId == 3);

        if (accountParams.Month.HasValue)
        {
            query = query.Where(x => x.Date_Created.Month == accountParams.Month);
        }

        // Filtering by year (optional)
        if (accountParams.Year.HasValue)
        {
            query = query.Where(x => x.Date_Created.Year == accountParams.Year);
        }

        query = query.OrderByDescending(a => a.Date_Created);

        return await PageList<AccountDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<AccountDto>(mapper.ConfigurationProvider),
            accountParams.PageNumber,
            accountParams.PageSize);
    }

    public async Task<PageList<ListRealEstateDto>> GetRealEstates(ReasParams reasParams)
    {
        var query = context.RealEstate.AsQueryable();

        if (reasParams.Month.HasValue)
        {
            query = query.Where(x => x.DateCreated.Month == reasParams.Month);
        }

        if (reasParams.Year.HasValue)
        {
            query = query.Where(x => x.DateCreated.Year == reasParams.Year);
        }

        /*query = query.Where(r => r.ReasStatus == 2);*/

        var realEstatesQuery = query.OrderByDescending(r => r.DateEnd)
            .Select(x => new ListRealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
        return await PageList<ListRealEstateDto>.CreateAsync(realEstatesQuery, reasParams.PageNumber,
            reasParams.PageSize);
    }

    public async Task<PageList<AuctionDto>> GetAuctions(AuctionParams auctionParams)
    {
        var query = context.Auction.AsQueryable();
        if (auctionParams.Month.HasValue)
        {
            query = query.Where(x => x.DateStart.Month == auctionParams.Month);
        }

        if (auctionParams.Year.HasValue)
        {
            query = query.Where(x => x.DateStart.Year == auctionParams.Year);
        }

        if (auctionParams.DateStart.HasValue)
        {
            query = query.Where(x => x.DateStart >= auctionParams.DateStart);
        }

        if (auctionParams.DateEnd.HasValue)
        {
            query = query.Where(x => x.DateEnd <= auctionParams.DateEnd);
        }

        var auctionQuery = query.OrderByDescending(au => au.DateStart)
            .Select(x => new AuctionDto
            {
                ReasId = x.ReasId,
                ReasName = context.RealEstate.Where(re => re.ReasId == x.ReasId).Select(re => re.ReasName)
                    .FirstOrDefault(),
                DateEnd = x.DateEnd,
                DateStart = x.DateStart,
                Status = x.Status,
                AuctionId = x.AuctionId,
                OwnerName = context.RealEstate.Where(re => re.ReasId == x.ReasId).Select(re => re.AccountOwnerName)
                    .FirstOrDefault(),
                AccountCreateName = x.AccountCreateName
            });

        return await PageList<AuctionDto>.CreateAsync(auctionQuery, auctionParams.PageNumber, auctionParams.PageSize);
    }

    public async Task<AuctionMoneyDto> GetAuctionMoney()
    {
        var auctionAccounting = await context.AuctionsAccounting.ToListAsync();

        // Calculate total commission and fee
        var totalFeeAmount = auctionAccounting
            .Select(aa => decimal.Parse(aa.MaxAmount) * 0.02m)
            .Sum();

        var totalCommissionAmount = auctionAccounting
            .Select(aa => decimal.Parse(aa.CommissionAmount))
            .Sum();

        var auctionMoneyDto = new AuctionMoneyDto
        {
            TotalFeeAmount = totalFeeAmount.ToString(),
            TotalCommissionAmount = totalCommissionAmount.ToString()
        };

        return auctionMoneyDto;
    }
}