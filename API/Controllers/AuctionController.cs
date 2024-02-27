using API.DTOs;
using API.Errors;
using API.Exceptions;
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

        public AuctionController(IAuctionService auctionService, IAuctionAccountingService auctionAccountingService)
        {
            _auctionService = auctionService;
            _auctionAccountingService = auctionAccountingService;
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

        //[Authorize(policy: "Customer")]
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
                    return BadRequest(new ApiResponse(404, "Update Auction Accounting fail"));
                }

                //update auction status
                int statusFinish = (int)AuctionEnum.Finish;
                bool result = await _auctionService.ToggleAuctionStatus(auctionDetailDto.AuctionId.ToString(), statusFinish.ToString());

                //send email
                //await _auctionAccountingService.SendWinnerEmail(auctionAccountingDto);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }
            //return calculate result in auction accounting

            return Ok(auctionAccountingDto);
        }

        [HttpGet("/owner/auction-history")]
        public async Task<IActionResult> GetOwnerAuctionHistory([FromQuery] AuctionHistoryParam auctionHisotoryParam)
        {
            try
            {
                var auctionHistory = await _auctionService.GetAuctionHisotoryForOwner(auctionHisotoryParam);

                Response.AddPaginationHeader(new PaginationHeader(auctionHistory.CurrentPage, auctionHistory.PageSize,
                auctionHistory.TotalCount, auctionHistory.TotalPages));

                return Ok(auctionHistory);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet("/owner/auction-history/{auctionId}")]
        public async Task<IActionResult> GetOwnerAuctionAccouting(int auctionId)
        {
            try
            {
                var auctionAccouting = await _auctionAccountingService.GetAuctionAccounting(auctionId);

                return Ok(auctionAccouting);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet("/attender/auction-history")]
        public async Task<IActionResult> GetAttenderAuctionHistory([FromQuery] AuctionHistoryParam auctionHisotoryParam)
        {
            try
            {
                var auctionHistory = await _auctionService.GetAuctionHisotoryForAttender(auctionHisotoryParam);

                Response.AddPaginationHeader(new PaginationHeader(auctionHistory.CurrentPage, auctionHistory.PageSize,
                auctionHistory.TotalCount, auctionHistory.TotalPages));

                return Ok(auctionHistory);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
