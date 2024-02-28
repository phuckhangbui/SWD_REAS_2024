using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using API.Validate;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstateRepository : BaseRepository<RealEstate>, IRealEstateRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RealEstateRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto)
        {
            var realEstate = await _context.RealEstate.Where(r => r.ReasId == reasStatusDto.Id).FirstOrDefaultAsync();
            if (realEstate != null)
            {
                realEstate.ReasStatus = reasStatusDto.status;
                realEstate.Message = reasStatusDto.statusMessage;
                try
                {
                    bool check = await UpdateAsync(realEstate);
                    if (check) return true;
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> CheckRealEstateExist(int reasId)
        {
            return await _context.RealEstateDetail.AnyAsync(r => r.ReasId == reasId);
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoing()
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(a => a.ReasStatus == (int)RealEstateEnum.In_progress).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoing()
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(a => a.ReasStatus != (int)RealEstateEnum.In_progress).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetOwnerRealEstate(int idOwner)
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(a => a.AccountOwnerId.Equals(idOwner)).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(x => x.ReasId == x.ReasId).Select(x => x.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetOwnerRealEstateBySearch(int idOwner, SearchRealEstateParam searchRealEstateDto)
        {
            ParseValidate parseValidate = new ParseValidate();
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            int? minPrice = null;
            int? maxPrice = null;
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom))
                minPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom);
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))
                maxPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo);

            var query = _context.RealEstate.Where(x => x.AccountOwnerId.Equals(idOwner) && ((searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))));
            if (minPrice != null)
            {
                string minPriceStr = minPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{minPriceStr}%"));
            }

            if (maxPrice != null)
            {
                string maxPriceStr = maxPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{maxPriceStr}%"));
            }
            var result = query.Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(x => x.ReasId == x.ReasId).Select(x => x.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            ParseValidate parseValidate = new();
            var page = new PaginationParams();
            int? minPrice = null;
            int? maxPrice = null;
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom))
                minPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom);
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))
                maxPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo);

            var query = _context.RealEstate.AsQueryable();
            query = query.Where(x =>
                ((new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Auctioning, (int)RealEstateEnum.Re_up }.Contains(x.ReasStatus) && searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))));

            // Xử lý giá trị ReasPrice: bỏ dấu
            if (minPrice != null)
            {
                string minPriceStr = minPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{minPriceStr}%"));
            }

            if (maxPrice != null)
            {
                string maxPriceStr = maxPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{maxPriceStr}%"));
            }
            query = query.OrderByDescending(a => a.DateStart);

            var result = query
                .Select(x => new RealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = x.ReasPrice,
                    ReasArea = x.ReasArea,
                    UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(y => y.ReasPhotoUrl).FirstOrDefault(),
                    ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd
                })
                .ToList();
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstateOnRealEstatePage()
        {
            var statusName = new GetStatusName();
            PaginationParams page = new PaginationParams();
            var query = _context.RealEstate.Where(x => new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Re_up, (int)RealEstateEnum.Auctioning }.Contains(x.ReasStatus)).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            }).AsQueryable();
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoingBySearch(SearchRealEstateParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var parseValidate = new ParseValidate();
            var page = new PaginationParams();
            int? minPrice = null;
            int? maxPrice = null;
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom))
                minPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom);
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))
                maxPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo);

            var query = _context.RealEstate.Where(x => (x.ReasStatus == (int)RealEstateEnum.In_progress) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))));
            if (minPrice != null)
            {
                string minPriceStr = minPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{minPriceStr}%"));
            }

            if (maxPrice != null)
            {
                string maxPriceStr = maxPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{maxPriceStr}%"));
            }
            var result = query.Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            }).ToList();
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingBySearch(SearchRealEstateParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var parseValidate = new ParseValidate();
            var page = new PaginationParams();
            int? minPrice = null;
            int? maxPrice = null;
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom))
                minPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom);
            if (!string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))
                maxPrice = (int?)parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo);

            var query = _context.RealEstate.Where(x => ((x.ReasStatus != (int)RealEstateEnum.In_progress && searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo))));
            if (minPrice != null)
            {
                string minPriceStr = minPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{minPriceStr}%"));
            }

            if (maxPrice != null)
            {
                string maxPriceStr = maxPrice.ToString();
                query = query.Where(x => EF.Functions.Like(x.ReasPrice.Replace(",", ""), $"%{maxPriceStr}%"));
            }
            var result = query.Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            }).ToList();
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public RealEstate GetRealEstate(int id)
        {
            return _context.RealEstate.Find(id);
        }
    }
}
