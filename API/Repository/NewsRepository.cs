using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public NewsRepository(DataContext context, IMapper mapper) : base(context)
        {
            _dataContext = context;
            _mapper = mapper;
        }

        public  async Task<News> CreateNewNewsByAdmin(NewsCreate newsDto, int id, string name)
        {
            try
            {
                News news = new News();
                news.Thumbnail = newsDto.ThumbnailUri;
                news.NewsTitle = newsDto.NewsTitle;
                news.NewsSumary = newsDto.NewsSumary;
                news.NewsContent = newsDto.NewsContent;
                news.AccountCreateId = id;
                news.DateCreated = DateTime.UtcNow;
                news.AccountName = name;
                CreateAsync(news);
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PageList<NewsDto>> GetAllInNewsPage()
        {
            PaginationParams paginationParams = new PaginationParams();
            var newsList = _dataContext.News.AsQueryable();
            newsList = newsList.OrderByDescending(x => x.DateCreated);
            if(newsList != null)
            {
                return await PageList<NewsDto>.CreateAsync(
                newsList.AsNoTracking().ProjectTo<NewsDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<NewsDetailDto> GetDetailOfNews(int id)
        {
            NewsDetailDto newsDetailDto = new NewsDetailDto();
            var newsDetail = await _dataContext.News.Where(x => x.NewsId == id).FirstOrDefaultAsync();
            newsDetailDto.NewsId = id;
            newsDetailDto.NewsTitle = newsDetail.NewsTitle;
            newsDetailDto.NewsSumary = newsDetail.NewsSumary;
            newsDetailDto.NewsContent = newsDetail.NewsContent;
            newsDetailDto.DateCreated = newsDetail.DateCreated;
            return newsDetailDto;
        }

        public async Task<PageList<NewsDto>> SearchNewByKey(SearchNewsParam searchNewsParam)
        {
            PaginationParams paginationParams = new PaginationParams();
            var newsList = _dataContext.News.AsQueryable();
            newsList = newsList.Where(x => x.NewsTitle.Contains(searchNewsParam.KeyWork) || 
            x.NewsSumary.Contains(searchNewsParam.KeyWork) ||
            x.NewsContent.Contains(searchNewsParam.KeyWork));
            newsList = newsList.OrderByDescending(x => x.DateCreated);
            if (newsList != null)
            {
                return await PageList<NewsDto>.CreateAsync(
                newsList.AsNoTracking().ProjectTo<NewsDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<News> UpdateNewsByAdmin(NewsDetailDto news)
        {
            var newsDto = await _dataContext.News.Where(x => x.NewsId == news.NewsId).FirstOrDefaultAsync();
            newsDto.NewsTitle= news.NewsTitle;
            newsDto.NewsSumary= news.NewsSumary;
            newsDto.NewsContent = news.NewsContent;
            newsDto.Thumbnail = news.Thumbnail;
            try
            {
                UpdateAsync(newsDto);
                return newsDto;
            }catch (Exception ex)
            {
                return null;
            }
        }
    }
}
