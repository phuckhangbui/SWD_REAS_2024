using System.Security.Claims;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Task = API.Entity.Task;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly IAdminRepository _adminRepository;
    private readonly IMapper _mapper;

    public AdminController(IAdminRepository adminRepository, IMapper mapper)
    {
        this._mapper = mapper;
        this._adminRepository = adminRepository;
    }

    [HttpGet("accounts")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts([FromQuery] AccountParams accountParams)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        var accounts = await _adminRepository.GetAccounts(accountParams);

        Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
            accounts.TotalCount, accounts.TotalPages));
        return Ok(accounts);
    }

    [HttpGet("real-estate")]
    public async Task<ActionResult<IEnumerable<ListRealEstateDto>>> GetRealEstate([FromQuery] ReasParams reasParams)
    {
        var reas = await _adminRepository.GetRealEstates(reasParams);
        Response.AddPaginationHeader(new PaginationHeader(reas.CurrentPage, reas.PageSize,
            reas.TotalCount, reas.TotalPages));
        return Ok(reas);
    }

    [HttpGet("auction")]
    public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuction([FromQuery] AuctionParams auctionParams)
    {
        var auction = await _adminRepository.GetAuctions(auctionParams);
        Response.AddPaginationHeader(new PaginationHeader(auction.CurrentPage, auction.PageSize,
            auction.TotalCount, auction.TotalPages));
        return Ok(auction);
    }

    [HttpGet("auction-money")]
    public async Task<ActionResult<IEnumerable<AuctionMoneyDto>>> GetAuctionMoney()
    {
        var auctionMoney = await _adminRepository.GetAuctionMoney();
        return Ok(auctionMoney);
    }
}