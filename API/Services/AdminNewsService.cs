using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using AutoMapper;

namespace API.Services
{
    public class AdminNewsService : BaseService<News>, IAdminNewsService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IPhotoService _photoService;
        public AdminNewsService(IAccountRepository accountRepository, IRealEstateRepository realEstateRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstatePhotoRepository realEstatePhotoRepository, INewsRepository newsRepository, IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository, IRuleRepository ruleRepository, ITypeReasRepository typeReasRepository, IAuctionRepository auctionRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper, IPhotoService photoService, ITokenService tokenService) : base(accountRepository, realEstateRepository, realEstateDetailRepository, realEstatePhotoRepository, newsRepository, moneyTransactionRepository, moneyTransactionDetailRepository, ruleRepository, typeReasRepository, auctionRepository, depositAmountRepository, mapper, photoService, tokenService)
        {
            _accountRepository = accountRepository;
            _newsRepository = newsRepository;
            _photoService = photoService;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<bool> AddNewNews(NewsCreate newCreate, int idAdmin)
        {
            //ConvertStringToFile convertStringToFile = new ConvertStringToFile();
            //IFormFile formFile = convertStringToFile.ConvertToIFormFile(newCreate.ThumbnailUri);
            //var result = await _photoService.AddPhotoNewsAsync(formFile, newCreate.NewsTitle);
            //if (result.Error != null)
            //{
            //    return false;
            //}

            //newCreate.ThumbnailUri = result.SecureUrl.AbsoluteUri;
            string name = await _accountRepository.GetNameAccountByAccountIdAsync(idAdmin);
            bool news = await _newsRepository.CreateNewNewsByAdmin(newCreate, idAdmin, name);
            if(news)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<NewsAdminDto>> GetAllNewsByAdmin()
        {
            var listNews = await _newsRepository.GetAllInNewsAdmin();
            return listNews;
        }

        public async Task<NewsDetailDto> GetNewsDetailByAdmin(int id)
        {
            var newsDetail = await _newsRepository.GetDetailOfNews(id);
            return newsDetail;
        }

        public async Task<IEnumerable<NewsAdminDto>> SearchNewsByAdmin(SearchNewsAdminParam searchNews)
        {
            var reals = await _newsRepository.SearchNewsAdminByKey(searchNews);
            return reals;
        }

        public async Task<bool> UpdateNewNews(NewsDetailDto newsDetailDto)
        {
            //ConvertStringToFile convertStringToFile = new ConvertStringToFile();
            //IFormFile formFile = convertStringToFile.ConvertToIFormFile(newsDetailDto.Thumbnail);
            //var result = await _photoService.AddPhotoNewsAsync(formFile, newsDetailDto.NewsTitle);
            //if (result.Error != null)
            //{
            //    return false;
            //}

            //newsDetailDto.Thumbnail = result.SecureUrl.AbsoluteUri;
            bool news = await _newsRepository.UpdateNewsByAdmin(newsDetailDto);
            if (news)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
