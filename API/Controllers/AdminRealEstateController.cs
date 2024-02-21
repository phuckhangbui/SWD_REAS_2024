using API.Enums;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class AdminRealEstateController : BaseApiController
	{
		private readonly IRealEstateRepository _realEstateRepository;
		private readonly IRealEstateDetailRepository _realEstateDetailRepository;

		private const string BaseUri = "/api/admin/realestates";
		private const string DetailUri = "/api/admin/realestates/{reasId}";
		private const string BlockUri = BaseUri + "/api/block/{reasId}";
		private const string UnblockUri = BaseUri + "/api/unblock/{reasId}";

		public AdminRealEstateController(
			IRealEstateRepository realEstateRepository,
			IRealEstateDetailRepository realEstateDetailRepository)
		{
			_realEstateRepository = realEstateRepository;
			_realEstateDetailRepository = realEstateDetailRepository;
		}

		[HttpGet(BaseUri)]
		public async Task<IActionResult> GetRealEstates([FromQuery] RealEstateParam realEstateParam)
		{
			var realEstates = await _realEstateRepository.GetRealEstatesAsync(realEstateParam);

			Response.AddPaginationHeader(new PaginationHeader(realEstates.CurrentPage, realEstates.PageSize,
			realEstates.TotalCount, realEstates.TotalPages));

			return Ok(realEstates);
		}

		[HttpGet(DetailUri)]
		public async Task<IActionResult> GetRealEstateDetail(int reasId)
		{
			bool isExist = await _realEstateRepository.CheckRealEstateExist(reasId);
			if (!isExist)
			{
				return BadRequest(new ApiException(404));
			}

			var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetailAsync(reasId);

			return Ok(realEstateDetailDto);
		}

		[HttpPost(BlockUri)]
		public async Task<IActionResult> BlockRealEstate(int reasId)
		{
			var realEstateDto = await _realEstateRepository.GetRealEstateWithStatusAsync(
				reasId, (int)RealEstateStatus.Selling);
			if (realEstateDto == null)
			{
				return BadRequest(new ApiException(404));
			}

			realEstateDto = await _realEstateRepository.UpdateRealEstateStatusAsync(reasId, (int)RealEstateStatus.Block);

			return Ok(realEstateDto);
		}

		[HttpPost(UnblockUri)]
		public async Task<IActionResult> UnblockRealEstate(int reasId)
		{
			var realEstateDto = await _realEstateRepository.GetRealEstateWithStatusAsync(
				reasId, (int)RealEstateStatus.Block);
			if (realEstateDto == null)
			{
				return BadRequest(new ApiException(404));
			}

			realEstateDto = await _realEstateRepository.UpdateRealEstateStatusAsync(reasId, (int)RealEstateStatus.Selling);

			return Ok(realEstateDto);
		}
	}
}
