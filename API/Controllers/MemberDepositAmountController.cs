using API.DTOs;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberDepositAmountController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDepositAmountRepository _depositAmountRepository;
        private const string BaseUri = "/api/home/";

        public MemberDepositAmountController(IDepositAmountRepository depositAmountRepository, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _depositAmountRepository = depositAmountRepository;
        }

        [HttpGet(BaseUri + "my-deposit")]
        public async Task<IActionResult> ListDepositAmoutByMember([FromQuery] PaginationParams paginationParams)
        {
            int userMember = GetIdMember(_accountRepository);
            var deposit = await _depositAmountRepository.GetDepositAmoutForMember(userMember);
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

        [HttpGet(BaseUri + "my-deposit/search")]
        public async Task<IActionResult> ListDepositAmoutByMemberWhenSearch([FromQuery] SearchDepositAmountDto searchDepositAmountDto)
        {
            int userMember = GetIdMember(_accountRepository);
            var deposit = await _depositAmountRepository.GetDepositAmoutForMemberBySearch(searchDepositAmountDto, userMember);
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
    }
}
