using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using API.MessageResponse;
using API.Validate;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateController : BaseApiController
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IAccountRepository _account_repository;
        private readonly IRealEstatePhotoRepository _real_estate_photo_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;

        public RealEstateController(IRealEstateRepository realEstateRepository, IAccountRepository accountRepository, IRealEstatePhotoRepository real_estate_photo_repository, IRealEstateDetailRepository real_estate_detail_repository)
        {
            _real_estate_repository = realEstateRepository;
            _account_repository = accountRepository;
            _real_estate_photo_repository = real_estate_photo_repository;
            _real_estate_detail_repository = real_estate_detail_repository;
        }

        [HttpGet("/home/real_estate")]
        public async Task<ActionResult<List<ListRealEstateDto>>> ListRealEstate()
        {
            var _real_estate_list = _real_estate_repository.GetAllAsync().Result.Where(x => new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Re_up, (int)RealEstateEnum.Auctioning }.Contains(x.ReasStatus)).Select(x => new ListRealEstateDto
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
            var _real_estate_list = _real_estate_repository.GetAllAsync().Result.Where(x =>
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
        public async Task<ActionResult<List<ListRealEstateDto>>> GetOnwerRealEstate()
        {
            int? userMember = GetLoginAccountId();
            if (userMember != null)
            {
                var list_owner_real_estate = _real_estate_repository.GetAllAsync().Result.Where(x => x.AccountOwnerId == userMember).Select(x => new ListRealEstateDto
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
                return Ok(list_owner_real_estate);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost("CreateRealEstate")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateDto newRealEstateDto)
        {
            int? userMember = GetLoginAccountId();
            var newRealEstate = new RealEstate();
            var newPhotoList = new RealEstatePhoto();
            var newDetail = new RealEstateDetail();
            newRealEstate.ReasName = newRealEstateDto.ReasName;
            newRealEstate.ReasPrice = newRealEstateDto.ReasPrice;
            newRealEstate.ReasAddress = newRealEstateDto.ReasAddress;
            newRealEstate.ReasDescription = newRealEstateDto.ReasDescription;
            newRealEstate.Message = "";
            newRealEstate.AccountOwnerId = userMember.Value;
            newRealEstate.DateCreated = DateTime.UtcNow;
            newRealEstate.DateStart = newRealEstateDto.DateStart;
            newRealEstate.DateEnd = newRealEstateDto.DateEnd;
            newRealEstate.ReasStatus = (int)RealEstateEnum.In_progress;
            newRealEstate.AccountOwnerName = _account_repository.GetAllAsync().Result.Where(x => x.AccountId == newRealEstateDto.AccountOwnerId).Select(x => x.AccountName).FirstOrDefault();
            _real_estate_repository.CreateAsync(newRealEstate);
            var idNewReal = _real_estate_repository.GetAllAsync().Result.OrderDescending().Select(x => x.ReasId).FirstOrDefault();
            foreach(ListPhotoRealEstateDto photos in newRealEstateDto.Photos)
            {
                newPhotoList.ReasPhotoUrl = photos.ReasPhotoUrl;
                newPhotoList.ReasId = idNewReal;
                _real_estate_photo_repository.CreateAsync(newPhotoList);
            }
            newDetail.Documents_Proving_Marital_Relationship = newRealEstateDto.Detail.Documents_Proving_Marital_Relationship;
            newDetail.Reas_Cert_Of_Land_Img_Front = newRealEstateDto.Detail.Reas_Cert_Of_Land_Img_Front;
            newDetail.Reas_Cert_Of_Land_Img_After = newRealEstateDto.Detail.Reas_Cert_Of_Land_Img_After;
            newDetail.Reas_Cert_Of_Home_Ownership = newRealEstateDto.Detail.Reas_Cert_Of_Home_Ownership;
            newDetail.ReasId = idNewReal;
            newDetail.Reas_Registration_Book = newRealEstateDto.Detail.Reas_Registration_Book;
            newDetail.Sales_Authorization_Contract = newRealEstateDto.Detail.Sales_Authorization_Contract;
            _real_estate_detail_repository.CreateAsync(newDetail);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return new ApiResponseMessage("MSG16");
        }
    }
}
