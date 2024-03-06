using API.DTOs;
using API.Entity;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Param.Enums;
using API.Services;
using CloudinaryDotNet;
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




        //for search also
        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("auctions/all")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctionsNotYetAndOnGoing()
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var auctions = await _auctionService.GetAuctionsNotYetAndOnGoing();

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            if(auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("auctions/all/detail/{id}")]
        public async Task<ActionResult<AuctionDetailOnGoing>> GetAuctionsDetailNotYetAndOnGoing(int id)
        {
            var auctions = await _auctionService.GetAuctionDetailOnGoing(id);

            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("auctions/complete")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctionsFinish()
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var auctions = await _auctionService.GetAuctionsFinish();

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("auctions/complete/detail/{id}")]
        public async Task<ActionResult<AuctionDetailFinish>> GetAuctionsDetailFinish(int id)
        {
            var auctions = await _auctionService.GetAuctionDetailFinish(id);

            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
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
                int statusFinish = (int)AuctionStatus.Finish;
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

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("realfordeposit")]
        public async Task<ActionResult<IEnumerable<ReasForAuctionDto>>> GetAuctionsReasForCreate()
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var real = await _auctionService.GetAuctionsReasForCreate();

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            if (real != null)
            {
                return Ok(real);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("realfordeposit/{id}")]
        public async Task<ActionResult<IEnumerable<DepositAmountUserDto>>> GetAllUserForDeposit(int id)
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var deposit = await _auctionService.GetAllUserForDeposit(id);

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            if (deposit != null)
            {
                return Ok(deposit);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpPost("deposit/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateAuction(AuctionCreateParam auctionCreateParam)
        {
            bool check = await _auctionService.CreateAuction(auctionCreateParam);
            if (check)
            {
                return new ApiResponseMessage("MSG05");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }

    }
}
