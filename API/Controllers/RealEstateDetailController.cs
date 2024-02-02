using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RealEstateDetailController : BaseApiController
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        private readonly IRealEstatePhotoRepository _real_estate_photo_repository;

        public RealEstateDetailController(IRealEstateRepository real_estate_repository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository) : base(realEstateDetailRepository)
        {
            _real_estate_repository = real_estate_repository;
            _real_estate_detail_repository = realEstateDetailRepository;
            _real_estate_photo_repository = realEstatePhotoRepository;
        }

        [HttpGet("/home/real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewRealEstateDetail(int id)
        {
            var _real_estate_detail = _real_estate_repository.GetAllAsync().Result.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                AccountOwnerId = x.AccountOwnerId,
                AccountOwnerName = x.AccountOwnerName,
                Detail = _real_estate_detail_repository.GetAllAsync().Result.Where(y => y.ReasId == x.ReasId).Select(y => new RealEstatePaper
                {
                    Reas_Cert_Of_Land_Img_Front = y.Reas_Cert_Of_Land_Img_Front,
                    Reas_Cert_Of_Land_Img_After = y.Reas_Cert_Of_Land_Img_After,
                    Reas_Cert_Of_Home_Ownership = y.Reas_Cert_Of_Home_Ownership,
                    Reas_Registration_Book = y.Reas_Registration_Book,
                    Documents_Proving_Marital_Relationship = y.Documents_Proving_Marital_Relationship
                }).FirstOrDefault(),
                Photos = _real_estate_photo_repository.GetAllAsync().Result.Where(z => z.ReasId == x.ReasId).Select(z => new ListPhotoRealEstateDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
            });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }

        [HttpGet("/home/my_real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewOwnerRealEstateDetail(int id)
        {
            var _real_estate_detail = _real_estate_repository.GetAllAsync().Result.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                Detail = _real_estate_detail_repository.GetAllAsync().Result.Where(y => y.ReasId == x.ReasId).Select(y => new RealEstatePaper
                {
                    Reas_Cert_Of_Land_Img_Front = y.Reas_Cert_Of_Land_Img_Front,
                    Reas_Cert_Of_Land_Img_After = y.Reas_Cert_Of_Land_Img_After,
                    Reas_Cert_Of_Home_Ownership = y.Reas_Cert_Of_Home_Ownership,
                    Reas_Registration_Book = y.Reas_Registration_Book,
                    Documents_Proving_Marital_Relationship = y.Documents_Proving_Marital_Relationship
                }).FirstOrDefault(),
                Photos = _real_estate_photo_repository.GetAllAsync().Result.Where(z => z.ReasId == x.ReasId).Select(z => new ListPhotoRealEstateDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
            });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }
    }
}
