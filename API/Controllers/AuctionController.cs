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
        public async Task<ActionResult<ConfirmAuctionSucessDto>> AuctionSuccess(AuctionDetailDto auctionDetailDto)
        {
            ConfirmAuctionSucessDto confirmAuctionSucessDto = new ConfirmAuctionSucessDto();
            try
            {
                //update/add auction accounting
                AuctionAccounting auctionAccounting = await _auctionAccountingService.UpdateAuctionAccounting(auctionDetailDto);

                if (auctionAccounting == null)
                {
                    return BadRequest(new ApiResponse(404, "Update Auction Accounting fail"));
                }

                //update auction status
                int statusFinish = (int)AuctionEnum.Finish;
                bool result = await _auctionService.ToggleAuctionStatus(auctionDetailDto.AuctionId.ToString(), statusFinish.ToString());

                //send email
                await _auctionAccountingService.SendWinnerEmail(auctionAccounting);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }
            //return calculate result in auction accounting

            return Ok(confirmAuctionSucessDto);
        }

    }
}
