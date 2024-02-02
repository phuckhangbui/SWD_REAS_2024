using System.Security.Claims;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts([FromQuery] AccountParams accountParams)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        var accounts = await _adminRepository.GetAccounts(accountParams);

        Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
            accounts.TotalCount, accounts.TotalPages));
        return Ok(accounts);
    }
}