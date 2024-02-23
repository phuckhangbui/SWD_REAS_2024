using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StaffRealEstateController : BaseApiController
    {
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private const string BaseUri = "/staff/";

        public StaffRealEstateController(IRealEstateRepository realEstateRepository, IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository)
        {
            _realEstateRepository = realEstateRepository;
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
        }

        [HttpGet(BaseUri + "real-estate/pending")]
        public async Task<IActionResult> GetRealEstateOnGoingByStaff([FromQuery] PaginationParams paginationParams)
        {
            int idStaff = GetIdStaff(_accountRepository);
            if (idStaff != 0)
            {
                var reals = await _realEstateRepository.GetRealEstateOnGoing();

                Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
                reals.TotalCount, reals.TotalPages));
                return Ok(reals);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/pending/detail/{id}")]
        public async Task<IActionResult> GetRealEstateOnGoingDetailByStaff(int id)
        {
            int idStaff = GetIdStaff(_accountRepository);
            if (idStaff != 0)
            {
                var real_estate_detail = _realEstateDetailRepository.GetRealEstateDetail(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(real_estate_detail);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            } 
        }

        [HttpGet(BaseUri + "real-estate/all")]
        public async Task<IActionResult> GetAllRealEstateExceptOnGoingByStaff([FromQuery] PaginationParams paginationParams)
        {
            int idStaff = GetIdStaff(_accountRepository);
            if (idStaff != 0)
            {
                var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoing();

                Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
                reals.TotalCount, reals.TotalPages));
                return Ok(reals);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/all/detail/{id}")]
        public async Task<IActionResult> GetRealEstateExceptOnGoingDetailByStaff(int id)
        {
            int idStaff = GetIdStaff(_accountRepository);
            if (idStaff != 0)
            {
                var real_estate_detail = _realEstateDetailRepository.GetRealEstateDetail(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(real_estate_detail);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "real-estate/pending/change")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateStatusRealEstateByStaff(ReasStatusDto reasStatusDto)
        {
            if (GetIdStaff(_accountRepository) != 0)
            {
                var updateReal = _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusDto);
                if (updateReal.Result != null)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
