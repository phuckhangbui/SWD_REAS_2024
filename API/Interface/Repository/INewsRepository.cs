using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<PageList<NewsDto>> GetAllInNewsPage();
        Task<PageList<NewsDto>> SearchNewByKey(SearchNewsParam searchNewsParam);
        Task<NewsDetailDto> GetDetailOfNews(int id);
        Task<bool> CreateNewNewsByAdmin(NewsCreate newCreate, int id, string name);
        Task<bool> UpdateNewsByAdmin(NewsDetailDto news);
    }
}
