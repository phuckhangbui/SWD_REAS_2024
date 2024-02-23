using API.DTOs;
using API.Enums;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminRealEstateController : BaseApiController
    {
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IAccountRepository _accountRepository;

        private const string BaseUri = "/admin/";

        public AdminRealEstateController(
            IRealEstateRepository realEstateRepository,
            IRealEstateDetailRepository realEstateDetailRepository,
            IAccountRepository accountRepository)
        {
            _realEstateRepository = realEstateRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _accountRepository = accountRepository;
        }

        [HttpPost(BaseUri + "real-estate/all/search")]
        public async Task<IActionResult> GetAllRealEstates([FromQuery] RealEstateParam realEstateParam)
        {
            var realEstates = await _realEstateRepository.GetRealEstatesAsync(realEstateParam);

            Response.AddPaginationHeader(new PaginationHeader(realEstates.CurrentPage, realEstates.PageSize,
            realEstates.TotalCount, realEstates.TotalPages));

            return Ok(realEstates);
        }

        [HttpPost(BaseUri + "real-estate/pending/search")]
        public async Task<IActionResult> GetAllRealEstatesPending([FromQuery] RealEstateParam realEstateParam)
        {
            var realEstates = await _realEstateRepository.GetRealEstatesAsync(realEstateParam);

            Response.AddPaginationHeader(new PaginationHeader(realEstates.CurrentPage, realEstates.PageSize,
            realEstates.TotalCount, realEstates.TotalPages));

            return Ok(realEstates);
        }

        [HttpGet(BaseUri + "real-estate/pending/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstatePendingDetail(int reasId)
        {
            bool isExist = await _realEstateRepository.CheckRealEstateExist(reasId);
            if (!isExist)
            {
                return BadRequest(new ApiException(404));
            }

            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetail(reasId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(realEstateDetailDto);
        }

        [HttpGet(BaseUri + "real-estate/all/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstateAllDetail(int reasId)
        {
            bool isExist = await _realEstateRepository.CheckRealEstateExist(reasId);
            if (!isExist)
            {
                return BadRequest(new ApiException(404));
            }

            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetail(reasId);

            return Ok(realEstateDetailDto);
        }

        [HttpPost(BaseUri + "real-estate/all/block")]
        public async Task<ActionResult<ApiResponseMessage>> BlockRealEstate(int reasId)
        {
            ReasStatusDto reasStatus = new ReasStatusDto();
            reasStatus.Id = reasId;
            reasStatus.status = (int)RealEstateEnum.Block;
            reasStatus.statusMessage = "";

            var updateReal = _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (updateReal.Result != null)
            {
                return new ApiResponseMessage("MSG03");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }

        [HttpPost(BaseUri + "real-estate/all/unblock")]
        public async Task<ActionResult<ApiResponseMessage>> UnblockRealEstate(int reasId)
        {
            ReasStatusDto reasStatus = new ReasStatusDto();
            reasStatus.Id = reasId;
            reasStatus.status = (int)RealEstateEnum.Selling;
            reasStatus.statusMessage = "";

            var updateReal = _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (updateReal.Result != null)
            {
                return new ApiResponseMessage("MSG03");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }

        [HttpGet(BaseUri + "real-estate/pending")]
        public async Task<IActionResult> GetRealEstateOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
        {
            int idAmin = GetIdAdmin(_accountRepository);
            if (idAmin != 0)
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

        [HttpGet(BaseUri + "real-estate/all")]
        public async Task<IActionResult> GetAllRealEstateExceptOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
        {
            int idAmin = GetIdAdmin(_accountRepository);
            if (idAmin != 0)
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

        [HttpPost(BaseUri + "real-estate/pending/change")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateStatusRealEstateByAdmin(ReasStatusDto reasStatusDto)
        {
            int idAdmin = 1;//GetIdAdmin(_accountRepository);
            if (idAdmin != 0)
            {
                var updateReal = _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusDto);
                if (updateReal.Result != null)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
