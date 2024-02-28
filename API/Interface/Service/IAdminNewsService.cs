using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAdminNewsService : IBaseService<News>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<NewsDto>> GetAllNewsByAdmin();
        Task<NewsDetailDto> GetNewsDetailByAdmin(int id);
        Task<PageList<NewsDto>> SearchNewsByAdmin(SearchNewsParam searchNews);
        Task<bool> AddNewNews(NewsCreate newCreate, int idAdmin);
        Task<bool> UpdateNewNews(NewsDetailDto newsDetailDto);
    }
}
