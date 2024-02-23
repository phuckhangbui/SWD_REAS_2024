using API.DTOs;
using API.Entity;
using API.Enums;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using API.Validate;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MemberRealEstateController : BaseApiController
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IAccountRepository _account_repository;
        private readonly IRealEstatePhotoRepository _real_estate_photo_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        private readonly IPhotoService _photoService;
        private readonly ITypeReasRepository _typeReasRepository;
        private const string BaseUri = "/home/";
        public MemberRealEstateController(IRealEstateRepository realEstateRepository,
            IAccountRepository accountRepository,
            IRealEstatePhotoRepository realEstatePhotoRepository,
            IRealEstateDetailRepository realEstateDetailRepository,
            IPhotoService photoService,
            ITypeReasRepository typeReasRepository)
        {
            _real_estate_repository = realEstateRepository;
            _account_repository = accountRepository;
            _real_estate_photo_repository = realEstatePhotoRepository;
            _real_estate_detail_repository = realEstateDetailRepository;
            _photoService = photoService;
            _typeReasRepository = typeReasRepository;
        }

        [HttpGet(BaseUri + "my_real_estate")]
        public async Task<ActionResult<List<RealEstateDto>>> GetOnwerRealEstate([FromQuery] PaginationParams paginationParams)
        {
            int userMember = GetIdMember(_account_repository);
            if (userMember != 0)
            {
                var reals = await _real_estate_repository.GetRealEstateOnGoing();
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

        [HttpGet(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<List<CreateNewRealEstatePage>>> ViewCreateNewRealEstatePage()
        {
            int userMember = GetIdMember(_account_repository);
            if (userMember != 0)
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

        [HttpPost(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateDto newRealEstateDto)
        {
            int userMember = GetIdMember(_account_repository);
            if (userMember != 0)
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
                newRealEstate.AccountOwnerId = userMember;
                newRealEstate.DateCreated = DateTime.UtcNow;
                newRealEstate.Type_Reas = newRealEstateDto.Type_Reas;
                newRealEstate.DateStart = newRealEstateDto.DateStart;
                newRealEstate.DateEnd = newRealEstateDto.DateEnd;
                newRealEstate.ReasStatus = (int)RealEstateEnum.In_progress;
                newRealEstate.AccountOwnerName = await _account_repository.GetNameAccountByAccountIdAsync(userMember);
                await _real_estate_repository.CreateAsync(newRealEstate);
                foreach (PhotoFileDto photos in newRealEstateDto.Photos)
                {
                    IFormFile formFile = convertStringToFile.ConvertToIFormFile(photos.ReasPhotoUrl);
                    var result = await _photoService.AddPhotoAsync(formFile, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    if (result.Error != null)
                    {
                        return BadRequest(result.Error.Message);
                    }

                    newPhotoList.ReasPhotoUrl = result.SecureUrl.AbsoluteUri;
                    newPhotoList.ReasId = newRealEstate.ReasId;
                    newPhotoList.ReasPhotoId = 0;

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
                    var documents_Proving_Marital_Relationship = await _photoService.AddPhotoAsync(file_documents_Proving_Marital_Relationship, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Land_Img_Front = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_Front, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Land_Img_After = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_After, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Cert_Of_Home_Ownership = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Home_Ownership, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var reas_Registration_Book = await _photoService.AddPhotoAsync(file_reas_Registration_Book, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    var sales_Authorization_Contract = await _photoService.AddPhotoAsync(file_sales_Authorization_Contract, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                    newDetail.Documents_Proving_Marital_Relationship = documents_Proving_Marital_Relationship.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Land_Img_Front = reas_Cert_Of_Land_Img_Front.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Land_Img_After = reas_Cert_Of_Land_Img_After.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Cert_Of_Home_Ownership = reas_Cert_Of_Home_Ownership.SecureUrl.AbsoluteUri;
                    newDetail.Reas_Registration_Book = reas_Registration_Book.SecureUrl.AbsoluteUri;
                    newDetail.Sales_Authorization_Contract = sales_Authorization_Contract.SecureUrl.AbsoluteUri;
                    newDetail.ReasId = newRealEstate.ReasId;
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

        [HttpGet(BaseUri + "my_real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewOwnerRealEstateDetail(int id)
        {
            int userMember = GetIdMember(_account_repository);
            if (userMember != 0)
            {
                var _real_estate_detail = _real_estate_detail_repository.GetRealEstateDetail(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(_real_estate_detail);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
