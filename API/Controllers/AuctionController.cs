using API.DTOs;
using API.Entity;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Param.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuctionController : BaseApiController
    {
        private readonly IAuctionService _auctionService;
        private readonly IAuctionAccountingService _auctionAccountingService;
        private readonly IDepositAmountService _depositAmountService;
        private readonly IMoneyTransactionService _moneyTransactionService;

        public AuctionController(IAuctionService auctionService, IAuctionAccountingService auctionAccountingService, IDepositAmountService depositAmountService, IMoneyTransactionService moneyTransactionService)
        {
            _auctionService = auctionService;
            _auctionAccountingService = auctionAccountingService;
            _depositAmountService = depositAmountService;
            _moneyTransactionService = moneyTransactionService;
        }

        [HttpGet("auctions")]
        public async Task<IActionResult> GetRealEstates([FromQuery] AuctionParam auctionParam)
        {
            var auctions = await _auctionService.GetRealEstates(auctionParam);

            Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
            auctions.TotalCount, auctions.TotalPages));

            return Ok(auctions);
        }




        //[HttpGet("/auctions")]
        //public async Task<ActionResult<List<Auction>>> GetAuctions()
        //{
        //    var auctions = await _auctionrepository.GetAllAsync();

        //    Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
        //    accounts.TotalCount, accounts.TotalPages));
        //    return Ok(auctions);
        //}


        //for search also
        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/auctions")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions(AuctionParam auctionParam)
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var auctions = await _auctionService.GetAuctions(auctionParam);

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
            auctions.TotalCount, auctions.TotalPages));
            return Ok(auctions);
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("edit/status")]
        public async Task<ActionResult<ApiResponseMessage>> ToggleAuctionStatus([FromQuery] string auctionId, string statusCode)
        {
            try
            {
                bool check = await _auctionService.ToggleAuctionStatus(auctionId, statusCode);
                if (check) return new ApiResponseMessage("MSG03");
                else return BadRequest(new ApiResponse(401, "Have an error when excute operation."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, "No AuctionId matched"));
            }

            return Ok();
        }

        [Authorize(policy: "Customer")]
        [HttpPost("success")]
        public async Task<ActionResult<AuctionAccountingDto>> AuctionSuccess(AuctionDetailDto auctionDetailDto)
        {
            AuctionAccountingDto auctionAccountingDto = new AuctionAccountingDto();
            try
            {
                //update/add auction accounting
                auctionAccountingDto = await _auctionAccountingService.UpdateAuctionAccounting(auctionDetailDto);

                if (auctionAccountingDto == null)
                {
                    return BadRequest(new ApiResponse(404, "Real estate is not auctioning"));
                }

                //update auction status
                int statusFinish = (int)AuctionEnum.Finish;
                bool result = await _auctionService.ToggleAuctionStatus(auctionDetailDto.AuctionId.ToString(), statusFinish.ToString());

                if (result)
                {
                    //send email
                    await _auctionAccountingService.SendWinnerEmail(auctionAccountingDto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }
            //return calculate result in auction accounting

            return Ok(auctionAccountingDto);
        }

        [Authorize(policy: "Customer")]
        [HttpGet("register")]
        public async Task<ActionResult<DepositAmountDto>> RegisterAuction([FromQuery] string customerId, string reasId)
        {
            if (GetLoginAccountId() != int.Parse(customerId))
            {
                return BadRequest(new ApiResponse(404));
            }
            DepositAmountDto depositAmountDto = new DepositAmountDto();

            try
            {
                depositAmountDto = await _depositAmountService.CreateDepositAmount(int.Parse(customerId), int.Parse(reasId));
                if (depositAmountDto == null)
                {
                    return BadRequest(new ApiResponse(404, "Real estate is not selling"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }


            return Ok(depositAmountDto);
        }

        //[Authorize(policy: "Customer")]
        [HttpPost("pay/deposit")]
        public async Task<ActionResult> PayAuctionDeposit(DepositPaymentDto paymentDto)
        {
            //if (GetLoginAccountId() != paymentDto.CustomerId)
            //{
            //    return BadRequest(new ApiResponse(404));
            //}
            try
            {
                //Get depositAmountDto

                DepositAmount depositAmount = _depositAmountService.GetDepositAmount(paymentDto.CustomerId, paymentDto.ReasId);

                if (depositAmount == null)
                {
                    return BadRequest(new ApiResponse(404, "Customer has not registered to bid in this real estate"));
                }

                if (depositAmount.Status != (int)UserDepositEnum.Pending)
                {
                    return BadRequest(new ApiResponse(404, "Customer has already paid the deposit"));

                }

                //create transaction and transaction detail

                if (paymentDto.Money != depositAmount.Amount)
                {
                    return BadRequest(new ApiResponse(404, "Amount of money is not matched"));
                }

                MoneyTransaction moneyTransaction = await _moneyTransactionService.CreateMoneyTransactionFromDepositPayment(paymentDto);

                //update depositAmount status
                DepositAmountDto depositAmountDto = await _depositAmountService.UpdateStatusToDeposited(paymentDto.CustomerId, paymentDto.ReasId, paymentDto.PaymentTime);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }

            return Ok("Payment Success!");
        }

    }
}
