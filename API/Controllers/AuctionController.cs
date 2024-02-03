using API.Extension;
using API.Helper;
using API.Interfaces;
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
	}
}
