using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class AdminAccountController : BaseApiController
{
    private readonly IAdminAccountService _adminAccountService;
    private const string BaseUri = "/api/admin/";

    public AdminAccountController(IAdminAccountService adminAccountService)
    {
        _adminAccountService = adminAccountService;
    }

    [HttpGet(BaseUri + "user/staff/search")]
    public async Task<ActionResult<IEnumerable<AccountStaffDto>>> GetStaffAccountsBySearch([FromQuery] AccountParams accountParams)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var accounts = await _adminAccountService.GetStaffAccountBySearch(accountParams);

            Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
                accounts.TotalCount, accounts.TotalPages));
            if (accounts.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(accounts);
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/member/search")]
    public async Task<ActionResult<IEnumerable<AccountMemberDto>>> GetMemberAccountsBySearch([FromQuery] AccountParams accountParams)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var accounts = await _adminAccountService.GetMemberAccountBySearch(accountParams);

            Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
                accounts.TotalCount, accounts.TotalPages));
            if (accounts.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(accounts);
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/staff")]
    public async Task<ActionResult<IEnumerable<AccountStaffDto>>> GetAllAccountStaffs([FromQuery] PaginationParams paginationParams)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var list_account = await _adminAccountService.GetStaffAccounts();
            if (list_account != null)
            {
                return BadRequest(new ApiResponse(404, "No data with your search"));
            }
            else
            {
                Response.AddPaginationHeader(new PaginationHeader(list_account.CurrentPage, list_account.PageSize,
                list_account.TotalCount, list_account.TotalPages));
                if (list_account.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(list_account);
                }
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/member")]
    public async Task<ActionResult<IEnumerable<AccountMemberDto>>> GetAllAccountMembers([FromQuery] PaginationParams paginationParams)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var list_account = await _adminAccountService.GetMemberAccounts();
            if (list_account != null)
            {
                return BadRequest(new ApiResponse(404, "No data with your search"));
            }
            else
            {
                Response.AddPaginationHeader(new PaginationHeader(list_account.CurrentPage, list_account.PageSize,
                list_account.TotalCount, list_account.TotalPages));
                if (list_account.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(list_account);
                }
            }
        }
        else
        {
            return BadRequest(new ApiResponse(401));
        }
    }

    [HttpGet(BaseUri + "user/staff/detail/{id}")]
    public async Task<ActionResult<StaffInformationDto>> GetStaffDetail(int id)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var accountStaff = await _adminAccountService.GetMemberDetail(id);
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
    public async Task<ActionResult<MemberInformationDto>> GetMemberDetail(int id)
    {
        var adminAccount = GetIdAdmin(_adminAccountService.AccountRepository);
        if (adminAccount != 0)
        {
            var accountMember = await _adminAccountService.GetMemberDetail(id);
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
    public async Task<ActionResult<ApiResponseMessage>> ChangeStatusAccount(ChangeStatusAccountParam changeStatusAccountDto)
    {
        var accountAdmin = GetIdAdmin(_adminAccountService.AccountRepository);
        if (accountAdmin != 0)
        {
            bool check = await _adminAccountService.ChangeStatusAccount(changeStatusAccountDto);
            if (check)
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
    public async Task<ActionResult<ApiResponseMessage>> CreateNewAccountForStaff(NewAccountParam account)
    {
        var accountAdmin = GetIdAdmin(_adminAccountService.AccountRepository);
        if (accountAdmin != 0)
        {
            if (await _adminAccountService.AccountRepository.isUserNameExisted(account.Username))
            {
                return BadRequest(new ApiResponse(400, "Username already exist"));
            }
            if (await _adminAccountService.AccountRepository.isEmailExisted(account.AccountEmail))
            {
                return BadRequest(new ApiResponse(400, "Email already exist"));
            }
            bool check = await _adminAccountService.CreateNewAccountForStaff(account);
            if (check)
            {
                return new ApiResponseMessage("MSG04");
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
}