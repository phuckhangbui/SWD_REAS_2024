using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Interfaces;
using API.MessageResponse;
using API.Services;
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
        private readonly IPhotoService _photoService;
        private readonly ITypeReasRepository _typeReasRepository;

        public RealEstateController(IRealEstateRepository realEstateRepository, IAccountRepository accountRepository, IRealEstatePhotoRepository real_estate_photo_repository, IRealEstateDetailRepository real_estate_detail_repository, IPhotoService photoService , ITypeReasRepository typeReasRepository)
        {
            _real_estate_repository = realEstateRepository;
            _account_repository = accountRepository;
            _real_estate_photo_repository = real_estate_photo_repository;
            _real_estate_detail_repository = real_estate_detail_repository;
            _photoService = photoService;
            _typeReasRepository = typeReasRepository;
        }

        [HttpGet("/home/real_estate")]
        public async Task<ActionResult<List<ListRealEstateDto>>> ListRealEstate()
        {
            var _real_estate_list = _real_estate_repository.GetAllAsync().Result.Where(x => new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Re_up, (int)RealEstateEnum.Auctioning }.Contains(x.ReasStatus)).Select(x => new ListRealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                ReasTypeName = _typeReasRepository.GetAllAsync().Result.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
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
                ((new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Auctioning, (int)RealEstateEnum.Re_up}.Contains(x.ReasStatus) && searchRealEstateForMemerDto.ReasStatus == -1) || searchRealEstateForMemerDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateForMemerDto.ReasName == null || x.ReasName.Contains(searchRealEstateForMemerDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateForMemerDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateForMemerDto.ReasPriceTo)) ||
                (parseValidate.ParseStringToInt(x.ReasPrice) >= parseValidate.ParseStringToInt(searchRealEstateForMemerDto.ReasPriceFrom) &&
                parseValidate.ParseStringToInt(x.ReasPrice) <= parseValidate.ParseStringToInt(searchRealEstateForMemerDto.ReasPriceTo))))
                .Select(x => new ListRealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = x.ReasPrice,
                    ReasArea = x.ReasArea,
                    ReasTypeName = _typeReasRepository.GetAllAsync().Result.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
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
                    ReasArea = x.ReasArea,
                    ReasTypeName = _typeReasRepository.GetAllAsync().Result.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = x.ReasStatus,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
                if (!list_owner_real_estate.Any())
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(list_owner_real_estate);
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

        [HttpGet("/home/my_real_estate/create")]
        public async Task<ActionResult<List<CreateNewRealEstatePage>>> ViewCreateNewRealEstatePage()
        {
            int? userMember = GetLoginAccountId();
            if (userMember != null)
            {
                var list_type_reas = _typeReasRepository.GetAllAsync().Result.Select(x => new CreateNewRealEstatePage
                {
                    TypeReasId = x.Type_ReasId,
                    TypeName = x.Type_Reas_Name,
                });
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_type_reas);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

            [HttpPost("CreateRealEstate")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateDto newRealEstateDto)
        {
            int? userMember = 23;//GetLoginAccountId();
            if (userMember != null)
            {
                var newRealEstate = new RealEstate();
                var newPhotoList = new RealEstatePhoto();
                var newDetail = new RealEstateDetail();
                ConvertStringToFile convertStringToFile = new ConvertStringToFile();
                newRealEstate.ReasName = newRealEstateDto.ReasName;
                newRealEstate.ReasPrice = newRealEstateDto.ReasPrice;
                newRealEstate.ReasAddress = newRealEstateDto.ReasAddress;
                newRealEstate.ReasArea = newRealEstateDto.ReasArea;
                newRealEstate.ReasDescription = newRealEstateDto.ReasDescription;
                newRealEstate.Message = "";
                newRealEstate.AccountOwnerId = userMember.Value;
                newRealEstate.DateCreated = DateTime.UtcNow;
                newRealEstate.Type_Reas = newRealEstateDto.Type_Reas;
                newRealEstate.DateStart = newRealEstateDto.DateStart;
                newRealEstate.DateEnd = newRealEstateDto.DateEnd;
                newRealEstate.ReasStatus = (int)RealEstateEnum.In_progress;
                newRealEstate.AccountOwnerName = _account_repository.GetAllAsync().Result.Where(x => x.AccountId == newRealEstateDto.AccountOwnerId).Select(x => x.AccountName).FirstOrDefault();
                await _real_estate_repository.CreateAsync(newRealEstate);
                var idNewReal = _real_estate_repository.GetAllAsync().Result.OrderByDescending(x => x.ReasId).Select(x => x.ReasId).FirstOrDefault();
                foreach (PhotoFileDto photos in newRealEstateDto.Photos)
                {
                    IFormFile formFile = convertStringToFile.ConvertToIFormFile(photos.ReasPhotoUrl);
                    var result = await _photoService.AddPhotoAsync(formFile, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    if (result.Error != null)
                    {
                        return BadRequest(result.Error.Message);
                    }

                    newPhotoList.ReasPhotoUrl = result.SecureUrl.AbsoluteUri;
                    newPhotoList.ReasId = idNewReal;

                    await _real_estate_photo_repository.CreateAsync(newPhotoList);
                }
                try
                {
                    IFormFile file_documents_Proving_Marital_Relationship = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Documents_Proving_Marital_Relationship);
                    IFormFile file_reas_Cert_Of_Land_Img_Front = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Reas_Cert_Of_Land_Img_Front);
                    IFormFile file_reas_Cert_Of_Land_Img_After = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Reas_Cert_Of_Land_Img_After);
                    IFormFile file_reas_Cert_Of_Home_Ownership = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Reas_Cert_Of_Home_Ownership);
                    IFormFile file_reas_Registration_Book = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Reas_Registration_Book);
                    IFormFile file_sales_Authorization_Contract = convertStringToFile.ConvertToIFormFile(newRealEstateDto.Detail.Sales_Authorization_Contract);
                    var documents_Proving_Marital_Relationship = await _photoService.AddPhotoAsync(file_documents_Proving_Marital_Relationship, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Land_Img_Front = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_Front, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Land_Img_After = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_After, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Home_Ownership = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Home_Ownership, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Registration_Book = await _photoService.AddPhotoAsync(file_reas_Registration_Book, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var sales_Authorization_Contract = await _photoService.AddPhotoAsync(file_sales_Authorization_Contract, idNewReal, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    newDetail.Documents_Proving_Marital_Relationship = documents_Proving_Marital_Relationship.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Land_Img_Front = reas_Cert_Of_Land_Img_Front.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Land_Img_After = reas_Cert_Of_Land_Img_After.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Home_Ownership = reas_Cert_Of_Home_Ownership.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Registration_Book = reas_Registration_Book.SecureUrl.AbsoluteUri;
                    newDetail.Sales_Authorization_Contract = sales_Authorization_Contract.SecureUrl.AbsoluteUri;
                    newDetail.ReasId = idNewReal;
                    await _real_estate_detail_repository.CreateAsync(newDetail);
                }
                catch (Exception ex)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return new ApiResponseMessage("MSG16");
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
