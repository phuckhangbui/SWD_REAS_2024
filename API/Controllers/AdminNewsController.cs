using API.DTOs;
using API.Entity;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using API.Services;
using API.Validate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace API.Controllers
{
    public class AdminNewsController : BaseApiController
    {
        private readonly INewsRepository _newsRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPhotoService _photoService;
        private const string BaseUri = "/api/admin/";

        public AdminNewsController(INewsRepository newsRepository, IAccountRepository accountRepository, IPhotoService photoService)
        {
            _accountRepository = accountRepository;
            _newsRepository = newsRepository;
            _photoService = photoService;
        }

        [HttpGet(BaseUri + "news")]
        public async Task<IActionResult> GetAllNewsByAdmin([FromQuery] PaginationParams paginationParams)
        {
            var listNews = await _newsRepository.GetAllInNewsPage();
            Response.AddPaginationHeader(new PaginationHeader(listNews.CurrentPage, listNews.PageSize,
            listNews.TotalCount, listNews.TotalPages));
            if (listNews.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(listNews);
            }
        }

        [HttpGet(BaseUri + "news/detail/{id}")]
        public async Task<IActionResult> GetNewsDetailByAdmin(int id)
        {
            var newsDetail = _newsRepository.GetDetailOfNews(id);
            if(newsDetail.Result != null)
            {
                return Ok(newsDetail);
            }
            else
            {
                return null;
            }
        }

        [HttpPost(BaseUri + "news/search")]
        public async Task<IActionResult> SearchNewsByAdmin(SearchNewsParam searchNews)
        {
            var reals = await _newsRepository.SearchNewByKey(searchNews);
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

        [HttpPost(BaseUri + "news/add")]
        public async Task<ActionResult<ApiResponseMessage>> AddNewNews(NewsCreate newCreate)
        {
            int idAdmin = GetIdAdmin(_accountRepository);
            if (idAdmin != 0)
            {
                ConvertStringToFile convertStringToFile = new ConvertStringToFile();
                IFormFile formFile = convertStringToFile.ConvertToIFormFile(newCreate.ThumbnailUri);
                var result = await _photoService.AddPhotoNewsAsync(formFile, newCreate.NewsTitle);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }

                newCreate.ThumbnailUri = result.SecureUrl.AbsoluteUri;
                string name = await _accountRepository.GetNameAccountByAccountIdAsync(idAdmin);
                var news = _newsRepository.CreateNewNewsByAdmin(newCreate, idAdmin, name);
                if (news != null)
                {
                    return new ApiResponseMessage("MSG18");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "news/update")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateNewNews(NewsDetailDto newsDetailDto)
        {
            int idAdmin = GetIdAdmin(_accountRepository);
            if (idAdmin != 0)
            {
                ConvertStringToFile convertStringToFile = new ConvertStringToFile();
                IFormFile formFile = convertStringToFile.ConvertToIFormFile(newsDetailDto.Thumbnail);
                var result = await _photoService.AddPhotoNewsAsync(formFile, newsDetailDto.NewsTitle);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }

                newsDetailDto.Thumbnail = result.SecureUrl.AbsoluteUri;
                var news = _newsRepository.UpdateNewsByAdmin(newsDetailDto);
                if (news != null)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
