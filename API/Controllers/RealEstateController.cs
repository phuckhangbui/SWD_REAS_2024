using API.DTOs;
using API.Extension;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateController : BaseApiController
    {
        private readonly IRealEstateService _realEstateService;
        private const string BaseUri = "/api/home/";

        public RealEstateController(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        [HttpGet(BaseUri + "real_estate")]
        public async Task<IActionResult> ListRealEstate([FromQuery] PaginationParams paginationParams)
        {
            var reals = await _realEstateService.ListRealEstate();
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
        public async Task<IActionResult> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto)
        {
            var reals = await _realEstateService.SearchRealEstateForMember(searchRealEstateDto);
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
            var _real_estate_detail = _realEstateService.ViewRealEstateDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }

    }
}
