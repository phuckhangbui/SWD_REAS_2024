using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class AdminRealEstateController : BaseApiController
	{
		private readonly IRealEstateRepository _realEstateRepository;
		private readonly IRealEstateDetailRepository _realEstateDetailRepository;
		private readonly IMapper _mapper;

		private const string BaseUri = "/admin/realestate";
		private const string DetailUri = "/admin/realestate/{reasId}";
		private const string BlockUri = BaseUri + "/block/{reasId}";
		private const string UnblockUri = BaseUri + "/unblock/{reasId}";

		public AdminRealEstateController(
			IRealEstateRepository realEstateRepository,
			IRealEstateDetailRepository realEstateDetailRepository,
			IMapper mapper)
		{
			_realEstateRepository = realEstateRepository;
			_realEstateDetailRepository = realEstateDetailRepository;
			_mapper = mapper;
		}

		[HttpGet(BaseUri)]
		public async Task<IActionResult> GetRealEstates()
		{
			var realEstates = await _realEstateRepository.GetRealEstatesAsync();

			var realEstateDtos = _mapper.Map<List<RealEstate>, List<RealEstateDto>>(realEstates);

			return Ok(realEstateDtos);
		}

		[HttpGet(DetailUri)]
		public async Task<IActionResult> GetRealEstateDetail(int reasId)
		{
			var realEstateDetail = await _realEstateDetailRepository.GetRealEstateDetailAsync(reasId);
			if (realEstateDetail == null)
			{
				return BadRequest(new ApiException(404));
			}

			var realEstateDetailDto = _mapper.Map<RealEstateInfoDto>(realEstateDetail);

			return Ok(realEstateDetailDto);
		}

		[HttpPost(BlockUri)]
		public async Task<IActionResult> BlockRealEstate(int reasId)
		{
			var realEstate = await _realEstateRepository.GetRealEstateWithStatusAsync(
				reasId, (int)RealEstateStatus.Selling);
			if (realEstate == null)
			{
				return BadRequest(new ApiException(404));
			}

			realEstate.ReasStatus = (int)RealEstateStatus.Block;
			//realEstate.DateEnd = DateTime.UtcNow;
			realEstate = await _realEstateRepository.UpdateRealEstateAsync(realEstate);

			var realEstateDto = _mapper.Map<RealEstateDto>(realEstate);
			return Ok(realEstateDto);
		}

		[HttpPost(UnblockUri)]
		public async Task<IActionResult> UnblockRealEstate(int reasId)
		{
			var realEstate = await _realEstateRepository.GetRealEstateWithStatusAsync(
				reasId, (int)RealEstateStatus.Block);
			if (realEstate == null)
			{
				return BadRequest(new ApiException(404));
			}

			realEstate.ReasStatus = (int)RealEstateStatus.Selling;
			//realEstate.DateEnd = DateTime.UtcNow;
			realEstate = await _realEstateRepository.UpdateRealEstateAsync(realEstate);

			var realEstateDto = _mapper.Map<RealEstateDto>(realEstate);
			return Ok(realEstateDto);
		}
	}
}
