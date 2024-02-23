using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<PageList<NewsDto>> GetAllInNewsPage();
        Task<PageList<NewsDto>> SearchNewByKey(SearchNewsParam searchNewsParam);
        Task<NewsDetailDto> GetDetailOfNews(int id);
        Task<News> CreateNewNewsByAdmin(NewsCreate newCreate, int id, string name);
        Task<News> UpdateNewsByAdmin(NewsDetailDto news);
    }
}
