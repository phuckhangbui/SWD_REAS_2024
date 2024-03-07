﻿using API.DTOs;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Param.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateController : BaseApiController
    {
        private readonly IRealEstateService _realEstateService;
        private readonly IDepositAmountService _depositAmountService;
        private const string BaseUri = "/api/home/";

        public RealEstateController(IRealEstateService realEstateService, IDepositAmountService depositAmountService)
        {
            _realEstateService = realEstateService;
            _depositAmountService = depositAmountService;
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
            var _real_estate_detail = await _realEstateService.ViewRealEstateDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }

        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "customer/auction/status")]
        public async Task<ActionResult<object>> CheckStatusOfUserWithTheRealEstate([FromQuery] string customerId, string reasId)
        {
            //
            // 5 statuses: 
            // 0: RealEstate not in selling status
            // 1: Not register in auction
            // 2: Register but pending payment
            // 3: Register success
            // 4: User is the owner of real estate

            if (GetLoginAccountId() != int.Parse(customerId))
            {
                return BadRequest(new ApiResponse(400));
            }

            var realEsateDetail = await _realEstateService.ViewRealEstateDetail(int.Parse(reasId));

            if (realEsateDetail == null)
            {
                return BadRequest(new ApiResponse(401));
            }

            if (realEsateDetail.AccountOwnerId == int.Parse(customerId))
            {
                return Ok(new
                {
                    message = "User is the onwer of real estate",
                    status = 4,
                });
            }

            if (realEsateDetail.ReasStatus != (int)RealEstateStatus.Selling)
            {
                return Ok(new
                {
                    message = "Real Estate is not for sale",
                    status = 0,
                });
            }

            var depositAmount = _depositAmountService.GetDepositAmount(int.Parse(customerId), int.Parse(reasId));

            if (depositAmount == null)
            {
                return Ok(new
                {
                    message = "User have not yet registered in auction",
                    status = 1
                });
            }

            if (depositAmount.Status == (int)(UserDepositEnum.Pending))
            {
                return Ok(new
                {
                    message = "Auction register is pending",
                    status = 2,
                    depositAmount = depositAmount
                });
            }

            if (depositAmount.Status == (int)(UserDepositEnum.Deposited))
            {
                return Ok(new
                {
                    message = "Auction register is success",
                    status = 3,
                    depositAmount = depositAmount
                });
            }

            return NoContent();


        }
    }
}
