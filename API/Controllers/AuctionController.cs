using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class AuctionController : BaseApiController
	{
		private readonly IAuctionRepository _auctionRepository;

		public AuctionController(IAuctionRepository auctionRepository)
		{
			_auctionRepository = auctionRepository;
		}

		private const string BaseUri = "/auctions";

		[HttpGet(BaseUri)]
		public async Task<IActionResult> GetRealEstates([FromQuery] AuctionParam auctionParam)
		{
			var auctions = await _auctionRepository.GetAuctionsAsync(auctionParam);

			Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
			auctions.TotalCount, auctions.TotalPages));

			return Ok(auctions);
		}


        [Authorize(policy: "AdminAndStaff")]

        //[HttpGet("/auctions")]
        //public async Task<ActionResult<List<Auction>>> GetAuctions()
        //{
        //    var auctions = await _auctionrepository.GetAllAsync();

        //    Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
        //    accounts.TotalCount, accounts.TotalPages));
        //    return Ok(auctions);
        //}


        //for search also
        [HttpGet("admin/auctions")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions(AuctionParam auctionParam)
        {
            //consider changing this to HttpPost

            //currently do not know search base on which properties
            var auctions = await _auctionRepository.GetAuctions(auctionParam);

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
                await _auctionRepository.EditAuctionStatus(auctionId, statusCode);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, "No AuctionId matched"));
            }

            return Ok();
        }


    }
}
