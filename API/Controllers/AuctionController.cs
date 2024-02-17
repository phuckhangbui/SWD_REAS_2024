using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(policy: "AdminAndStaff")]
    public class AuctionController : BaseApiController
    {
        private IAuctionRepository _auctionrepository;
        public AuctionController(IAuctionRepository auctionRepository)
        {
            _auctionrepository = auctionRepository;
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
        [HttpGet("/auctions")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions(AuctionParam auctionParam)
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var auctions = await _auctionrepository.GetAuctions(auctionParam);

            //need to test the mapper here
            //currently expect mapper to auto flatten the object, but let see :0
            Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
            auctions.TotalCount, auctions.TotalPages));
            return Ok(auctions);
        }

        [HttpGet("/edit/status")]
        public async Task<ActionResult> ToggleAuctionStatus([FromQuery] string auctionId, string statusCode)
        {
            try
            {
                await _auctionrepository.EditAuctionStatus(auctionId, statusCode);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, "No AuctionId matched"));
            }

            return Ok();
        }


    }
}
