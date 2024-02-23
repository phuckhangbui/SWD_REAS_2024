using API.DTOs;
using API.Extension;
using API.Helper;
using API.Interfaces;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly INewsRepository _newsRepository;
        private const string BaseUri = "/home/";

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet(BaseUri + "news")]
        public async Task<IActionResult> GetAllNews([FromQuery] PaginationParams paginationParams)
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
        public async Task<IActionResult> GetNewsDetail(int id)
        {
            var newsDetail = _newsRepository.GetDetailOfNews(id);
            if (newsDetail.Result != null)
            {
                return Ok(newsDetail);
            }
            else
            {
                return null;
            }
        }

        [HttpPost(BaseUri + "news/search")]
        public async Task<IActionResult> SearchNews(SearchNewsParam searchNews)
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
    }
}
