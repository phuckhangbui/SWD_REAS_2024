using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstateDetailRepository : BaseRepository<RealEstateDetail>, IRealEstateDetailRepository
    {
		private readonly DataContext _context;

		public RealEstateDetailRepository(DataContext context) : base(context)
		{
			_context = context;
		}

        public async Task<RealEstateDetailDto> GetRealEstateDetail(int id)
        {
            var _real_estate_detail = _context.RealEstate.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = x.ReasPrice,
                ReasArea = x.ReasArea,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                AccountOwnerId = x.AccountOwnerId,
                AccountOwnerName = x.AccountOwnerName,
                Detail = null,
                Photos = _context.RealEstatePhoto.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePhotoDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
            }).FirstOrDefaultAsync();
            return await _real_estate_detail;
        }
    }
}
