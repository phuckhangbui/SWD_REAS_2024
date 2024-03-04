using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberRealEstateController : BaseApiController
    {
        private readonly IMemberRealEstateService _memberRealEstateService;
        private const string BaseUri = "/api/home/";
        public MemberRealEstateController(IMemberRealEstateService memberRealEstateService)
        {
            _memberRealEstateService = memberRealEstateService;
        }

        [HttpGet(BaseUri + "my_real_estate")]
        public async Task<ActionResult<RealEstateDto>> GetOnwerRealEstate([FromQuery] PaginationParams paginationParams)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var reals = await _memberRealEstateService.GetOnwerRealEstate(userMember);
                Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
                reals.TotalCount, reals.TotalPages));
                if (reals.PageSize != 0)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    return BadRequest(new ApiResponse(404, "No data with your search"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/search")]
        public async Task<IActionResult> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var reals = await _memberRealEstateService.SearchOwnerRealEstateForMember(searchRealEstateParam, userMember);
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
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage()
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var list_type_reas = await _memberRealEstateService.ViewCreateNewRealEstatePage();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_type_reas);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateParam newRealEstateDto)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                bool flag = await _memberRealEstateService.CreateNewRealEstate(newRealEstateDto, userMember);
                if (flag)
                    return new ApiResponseMessage("MSG16");
                else
                {
                    return BadRequest(new ApiResponse(400,"Have any error when excute operation."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "my_real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewOwnerRealEstateDetail(int id)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var _real_estate_detail = _memberRealEstateService.ViewOwnerRealEstateDetail(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(_real_estate_detail);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/payment")]
        public async Task<ActionResult<ApiResponseMessage>> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                if(transactionMoneyCreateParam.Money != transactionMoneyCreateParam.MoneyPaid)
                {
                    return new ApiResponseMessage("MSG20");
                }
                else
                {
                    try
                    {
                        bool check = await _memberRealEstateService.PaymentAmountToUpRealEstaeAfterApprove(transactionMoneyCreateParam, userMember);
                        if (check)
                            return new ApiResponseMessage("MSG19");
                        else return BadRequest(new ApiResponse(401, "Have any error when excute operation"));
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                    }
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
