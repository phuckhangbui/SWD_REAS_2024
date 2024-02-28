using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DepositAmountController : BaseApiController
    {
        private readonly IDepositAmountService _depositAmountService;

        public DepositAmountController(IDepositAmountService depositAmountService)
        {
            _depositAmountService = depositAmountService;
        }

        private const string BaseUri = "/api/deposits";

        [HttpGet(BaseUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetDepositAmounts([FromQuery] DepositAmountParam depositAmountParam)
        {
            try
            {
                var depositAmounts = await _depositAmountService.GetDepositAmounts(depositAmountParam);

                Response.AddPaginationHeader(new PaginationHeader(depositAmounts.CurrentPage, depositAmounts.PageSize,
                depositAmounts.TotalCount, depositAmounts.TotalPages));

                return Ok(depositAmounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
