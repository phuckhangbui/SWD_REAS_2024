﻿using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface INewsService : IBaseService<News>
    {
        Task<PageList<NewsDto>> GetAllNews();
        Task<NewsDetailDto> GetNewsDetail(int id);
        Task<PageList<NewsDto>> SearchNews(SearchNewsParam searchNews);
    }
}
