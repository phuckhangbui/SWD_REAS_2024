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
}