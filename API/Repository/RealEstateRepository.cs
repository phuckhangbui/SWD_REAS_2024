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
    public class RealEstateRepository : BaseRepository<RealEstate>, IRealEstateRepository
    {

        private readonly DataContext _context;
		private readonly IMapper _mapper;

        public RealEstateRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
			_mapper = mapper;
        }

		public async Task<RealEstateDto> GetRealEstateAsync(int reasId)
		{
			var realEstate = await _context.RealEstate.SingleOrDefaultAsync(r => r.ReasId == reasId);
			return _mapper.Map<RealEstateDto>(realEstate);
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

		public async Task<RealEstateDto> GetRealEstateWithStatusAsync(int reasId, int status)
		{
			var realEstate =  await _context.RealEstate.SingleOrDefaultAsync(r => r.ReasId == reasId && r.ReasStatus == status);
			return _mapper.Map<RealEstateDto>(realEstate);
		}

		public async Task<RealEstateDto> UpdateRealEstateStatusAsync(int reasId, int status)
		{
			var realEstate = await _context.RealEstate.SingleOrDefaultAsync(r => r.ReasId == reasId);
			if (realEstate != null)
			{
				realEstate.ReasStatus = status;
				_context.Entry(realEstate).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return _mapper.Map<RealEstateDto>(realEstate);
			}

			return null;
		}

		public async Task<bool> CheckRealEstateExist(int reasId)
		{
			return await _context.RealEstateDetail.AnyAsync(r => r.ReasId == reasId);
		}
	}
}
