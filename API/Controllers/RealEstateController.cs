using API.DTOs;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateController : BaseApiController
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        private const string BaseUri = "/api/home/";

        public RealEstateController(IRealEstateRepository realEstateRepository, IRealEstateDetailRepository real_estate_detail_repository)
        {
            _real_estate_repository = realEstateRepository;
            _real_estate_detail_repository = real_estate_detail_repository;
        }

        [HttpGet(BaseUri + "real_estate")]
        public async Task<IActionResult> ListRealEstate([FromQuery] PaginationParams paginationParams)
        {
            var reals = await _real_estate_repository.GetAllRealEstateOnRealEstatePage();
            Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
            reals.TotalCount, reals.TotalPages));
            if (reals.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(reals);
            }
        }

        [HttpPost(BaseUri + "real_estate/search")]
        public async Task<IActionResult> SearchRealEstateForMember(SearchRealEstateDto searchRealEstateDto)
        {
            var reals = await _real_estate_repository.SearchRealEstateByKey(searchRealEstateDto);
            Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
            reals.TotalCount, reals.TotalPages));
            if (reals.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(reals);
            }
        }

        [HttpGet(BaseUri + "real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewRealEstateDetail(int id)
        {
            var _real_estate_detail = _real_estate_detail_repository.GetRealEstateDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }

    }
}
