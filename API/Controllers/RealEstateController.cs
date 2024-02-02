using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using API.Validate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IRealEstateRepository _real_estate_repository;

        public RealEstateController(IRealEstateRepository real_estate_repository, DataContext context)
        {
            _real_estate_repository = real_estate_repository;
            _context = context;
        }

        [HttpGet("/home/real_estate")]
        public async Task<ActionResult<List<ListRealEstateDto>>> ManageRealEstate()
        {
            var _real_estate_list = _real_estate_repository.GetAll().Where(x => new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Re_up, (int)RealEstateEnum.Auctioning }.Contains(x.ReasStatus)).Select(x => new ListRealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_list);
        }

        [HttpPost("searchRealEstate")]
        public async Task<ActionResult<List<ListRealEstateDto>>> SearchRealEstateForMember(SearchRealEstateForMemerDto searchRealEstateForMemerDto)
        {
            ParseValidate parseValidate = new ParseValidate();
            var _real_estate_list = _real_estate_repository.GetAll().Where(x =>
                ((new[] { 2, 4, 7 }.Contains(x.ReasStatus) && searchRealEstateForMemerDto.ReasStatus == -1) || searchRealEstateForMemerDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateForMemerDto.ReasName == null || x.ReasName.Contains(searchRealEstateForMemerDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateForMemerDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateForMemerDto.ReasPriceTo)) ||
                (parseValidate.ParseStringToInt(x.ReasPrice) >= parseValidate.ParseStringToInt(searchRealEstateForMemerDto.ReasPriceFrom) &&
                parseValidate.ParseStringToInt(x.ReasPrice) <= parseValidate.ParseStringToInt(searchRealEstateForMemerDto.ReasPriceTo))))
                .Select(x => new ListRealEstateDto
                {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            if (!_real_estate_list.Any())
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(_real_estate_list);
            }
        }
        [HttpGet("/home/my_real_estate")]
        public async Task<ActionResult<List<ListRealEstateDto>>> GetOnwerRealEstate(UserDto userDto)
        {
            if(userDto != null)
            {
                var list_owner_real_estate = _real_estate_repository.GetAll().Where(x => x.AccountOwnerId == userDto.AccountId).Select(x => new ListRealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = x.ReasPrice,
                    ReasStatus = x.ReasStatus,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
                return list_owner_real_estate.ToList();
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

           
    }
}
