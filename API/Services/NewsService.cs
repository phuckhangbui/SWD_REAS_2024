using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using AutoMapper;

namespace API.Services
{
    public class NewsService : BaseService<News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _newsRepository = newsRepository;
        }

        public async Task<PageList<NewsDto>> GetAllNews()
        {
            var listNews = await _newsRepository.GetAllInNewsPage();
            return listNews;
        }

        public async Task<NewsDetailDto> GetNewsDetail(int id)
        {
            var newsDetail = await _newsRepository.GetDetailOfNews(id);
            return newsDetail;
        }

        public async Task<PageList<NewsDto>> SearchNews(SearchNewsParam searchNews)
        {
            var reals = await _newsRepository.SearchNewByKey(searchNews);
            return reals;
        }
    }
}
