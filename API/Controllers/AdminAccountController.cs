using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AdminAccountController : BaseApiController
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private const string BaseUri = "/admin/";

    public AdminAccountController(IMapper mapper, IAccountRepository accountRepository)
    {
        this._mapper = mapper;
        _accountRepository = accountRepository;
    }

    [HttpGet(BaseUri + "user/staff/search")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetStaffAccountsBySearch([FromQuery] AccountParams accountParams)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        accountParams.RoleID = (int)RoleEnum.Staff;
        var accounts = await _accountRepository.GetAccountsBySearch(accountParams);

        Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
            accounts.TotalCount, accounts.TotalPages));
        return Ok(accounts);
    }

    [HttpGet(BaseUri + "user/member/search")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetMemberAccountsBySearch([FromQuery] AccountParams accountParams)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        accountParams.RoleID = (int)RoleEnum.Member;
        var accounts = await _accountRepository.GetAccountsBySearch(accountParams);

        Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
            accounts.TotalCount, accounts.TotalPages));
        return Ok(accounts);
    }

    [HttpGet(BaseUri + "user/staff")]
    public async Task<ActionResult<List<AccountListDto>>> GetAllAccountStaffs([FromQuery] PaginationParams paginationParams)
    {
        var adminAccount = GetIdAdmin(_accountRepository);
        if (adminAccount != 0)
        {
            var list_account = _accountRepository.GetAllStaffAccounts();
            if (list_account != null)
            {
                return BadRequest(new ApiResponse(404, "No data with your search"));
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/member")]
    public async Task<ActionResult<List<AccountListDto>>> GetAllAccountMembers([FromQuery] PaginationParams paginationParams)
    {
        var adminAccount = GetIdAdmin(_accountRepository);
        if (adminAccount != 0)
        {
            var list_account = _accountRepository.GetAllMemberAccounts();
            if (list_account != null)
            {
                return BadRequest(new ApiResponse(404, "No data with your search"));
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/staff/detail/{id}")]
    public async Task<ActionResult<UserInformationDto>> GetStaffDetail(int id)
    {
        var adminAccount = GetIdAdmin(_accountRepository);
        if (adminAccount != 0)
        {
            var accountStaff = _accountRepository.GetAccountDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(accountStaff);
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/member/detail/{id}")]
    public async Task<ActionResult<UserInformationDto>> GetMemberDetail(int id)
    {
        var adminAccount = GetIdAdmin(_accountRepository);
        if (adminAccount != 0)
        {
            var accountMember = _accountRepository.GetAccountDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(accountMember);
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }


    [HttpPost(BaseUri + "user/member/change")]
    public async Task<ActionResult<ApiResponseMessage>> ChangeStatusAccount(ChangeStatusAccountDto changeStatusAccountDto)
    {
        var accountAdmin = GetIdAdmin(_accountRepository);
        if (accountAdmin != 0)
        {
            var account = _accountRepository.UpdateStatusAccount(changeStatusAccountDto);
            if (account.Result != null)
            {
                return new ApiResponseMessage("MSG17");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpPost(BaseUri + "user/create")]
    public async Task<ActionResult<ApiResponseMessage>> CreateNewAccountForStaff(NewAccountDto account)
    {
        var accountAdmin = GetIdAdmin(_accountRepository);
        if (accountAdmin != 0)
        {
            if (await _accountRepository.isUserNameExisted(account.Username))
            {
                return BadRequest(new ApiResponse(400, "Username already exist"));
            }
            if (await _accountRepository.isEmailExisted(account.AccountEmail))
            {
                return BadRequest(new ApiResponse(400, "Email already exist"));
            }
            var newaccount = new Account();
            using var hmac = new HMACSHA512();
            newaccount.Username = account.Username;
            newaccount.AccountEmail = account.AccountEmail;
            newaccount.Address = account.Address;
            newaccount.AccountName = account.AccountName;
            newaccount.Citizen_identification = account.Citizen_identification;
            newaccount.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(account.PasswordHash));
            newaccount.PasswordSalt = hmac.Key;
            newaccount.PhoneNumber = account.PhoneNumber;
            newaccount.RoleId = 2;
            newaccount.Date_Created = DateTime.UtcNow;
            newaccount.Date_End = DateTime.MaxValue;
            newaccount.Account_Status = 1;
            _accountRepository.CreateAsync(newaccount);
            SendMailNewStaff.SendEmailWhenCreateNewStaff(account.AccountEmail, account.Username, account.PasswordHash, account.AccountName);
            return new ApiResponseMessage("MSG04");
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }
}