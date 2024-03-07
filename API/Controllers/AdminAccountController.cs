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
            if (accounts != null)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(accounts);
            }
            else
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
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

            if (accounts != null)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(accounts);
            }
            else
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
            else
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_account);
            }
            else
            {
            var apiResponseMessage = new ApiResponseMessage("MSG01");
            return Ok(new List<ApiResponseMessage> { apiResponseMessage });
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
            var accountStaff = await _adminAccountService.GetStaffDetail(id);
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


    [HttpPost(BaseUri + "user/change")]
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
                return new ApiResponseMessage("MSG22");
            }
            if (await _adminAccountService.AccountRepository.isEmailExistedCreateAccount(account.AccountEmail))
            {
                return new ApiResponseMessage("MSG23");
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