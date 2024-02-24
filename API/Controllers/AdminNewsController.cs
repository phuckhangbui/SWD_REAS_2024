using API.DTOs;
using API.Entity;
using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace API.Controllers
{
    public class AdminNewsController : BaseApiController
    {
        private readonly INewsRepository _newsRepository;
        private readonly IAccountRepository _accountRepository;
        private const string BaseUri = "/api/admin/";

        public AdminNewsController(INewsRepository newsRepository, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _newsRepository = newsRepository;
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
