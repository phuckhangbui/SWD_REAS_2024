﻿using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberDepositAmountController : BaseApiController
    {
        private readonly IMemberDepositAmountService _memberDepositAmountService;
        private const string BaseUri = "/api/home/";

        public MemberDepositAmountController(IMemberDepositAmountService memberDepositAmountService)
        {
            _memberDepositAmountService = memberDepositAmountService;
        }

        [HttpGet(BaseUri + "my-deposit")]
        public async Task<IActionResult> ListDepositAmoutByMember([FromQuery] PaginationParams paginationParams)
        {
            int userMember = GetIdMember(_memberDepositAmountService.AccountRepository);
            if(userMember != 0)
            {
                var deposit = await _memberDepositAmountService.ListDepositAmoutByMember(userMember);
                Response.AddPaginationHeader(new PaginationHeader(deposit.CurrentPage, deposit.PageSize,
                deposit.TotalCount, deposit.TotalPages));
                if (deposit.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(deposit);
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "my-deposit/search")]
        public async Task<IActionResult> ListDepositAmoutByMemberWhenSearch([FromQuery] SearchDepositAmountParam searchDepositAmountParam)
        {
            int userMember = GetIdMember(_memberDepositAmountService.AccountRepository);
            if(userMember != 0)
            {
                var deposit = await _memberDepositAmountService.ListDepositAmoutByMemberWhenSearch(searchDepositAmountParam, userMember);
                Response.AddPaginationHeader(new PaginationHeader(deposit.CurrentPage, deposit.PageSize,
                deposit.TotalCount, deposit.TotalPages));
                if (deposit.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(deposit);
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
