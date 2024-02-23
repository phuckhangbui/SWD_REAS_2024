using API.Data;
using API.DTOs;
using API.Entity;
using API.Enums;
using API.Helper;
using API.Interfaces;
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

		public async Task<PageList<RealEstateDto>> GetRealEstatesAsync(RealEstateParam realEstateParam)
		{
			var query = _context.RealEstate.AsQueryable();

			if (!string.IsNullOrEmpty(realEstateParam.Keyword))
			{
				query = query.Where(r => 
					r.ReasName.ToLower().Contains(realEstateParam.Keyword.ToLower()) ||
					r.ReasAddress.ToLower().Contains(realEstateParam.Keyword.ToLower()) ||
					r.AccountOwner.AccountName.ToLower().Contains(realEstateParam.Keyword.ToLower()));
			}

			query = query.OrderByDescending(r => r.DateCreated);

			return await PageList<RealEstateDto>.CreateAsync(
			query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
			realEstateParam.PageNumber,
			realEstateParam.PageSize);
		}

		public async Task<ReasStatusDto> UpdateRealEstateStatusAsync(ReasStatusDto reasStatusDto)
		{
			var realEstate = await _context.RealEstate.Where(r => r.ReasId == reasStatusDto.Id).FirstOrDefaultAsync();
			if (realEstate != null)
			{
				realEstate.ReasStatus = reasStatusDto.status;
				realEstate.Message = reasStatusDto.statusMessage;
                try
                {
                    await UpdateAsync(realEstate);
                    return reasStatusDto;
                }catch (Exception ex)
                {
                    return null;
                }
			}

			return null;
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

        public async Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateDto searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            ParseValidate parseValidate = new ParseValidate();
            var page = new PaginationParams();
            var query = _context.RealEstate.AsQueryable();
            query = (IQueryable<RealEstate>)query.Where(x =>
                ((new[] { (int)RealEstateEnum.Selling, (int)RealEstateEnum.Auctioning, (int)RealEstateEnum.Re_up }.Contains(x.ReasStatus) && searchRealEstateDto.ReasStatus == -1) 
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo)) ||
                (parseValidate.ParseStringToInt(x.ReasPrice) >= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom) &&
                parseValidate.ParseStringToInt(x.ReasPrice) <= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo))))
                .Select(x => new RealEstateDto
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

        public async Task<PageList<RealEstateDto>> GetRealEstateOnGoingBySearch(SearchRealEstateDto searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var parseValidate = new ParseValidate();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(x => (x.ReasStatus == (int)RealEstateEnum.In_progress) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo)) ||
                (parseValidate.ParseStringToInt(x.ReasPrice) >= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom) &&
                parseValidate.ParseStringToInt(x.ReasPrice) <= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo))))
                .Select(x => new RealEstateDto
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

        public async Task<PageList<RealEstateDto>> GetAllRealEstateExceptOnGoingBySearch(SearchRealEstateDto searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var parseValidate = new ParseValidate();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(x => ((x.ReasStatus != (int)RealEstateEnum.In_progress && searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((string.IsNullOrEmpty(searchRealEstateDto.ReasPriceFrom) && string.IsNullOrEmpty(searchRealEstateDto.ReasPriceTo)) ||
                (parseValidate.ParseStringToInt(x.ReasPrice) >= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceFrom) &&
                parseValidate.ParseStringToInt(x.ReasPrice) <= parseValidate.ParseStringToInt(searchRealEstateDto.ReasPriceTo))))
                .Select(x => new RealEstateDto
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
    }
}
