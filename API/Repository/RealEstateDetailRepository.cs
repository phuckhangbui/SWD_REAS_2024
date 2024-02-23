using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstateDetailRepository : BaseRepository<RealEstateDetail>, IRealEstateDetailRepository
    {
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public RealEstateDetailRepository(DataContext context, IMapper mapper) : base(context)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<RealEstateInfoDto> GetRealEstateDetailAsync(int realsId)
		{
			var realEstateDetail = 
				await _context.RealEstateDetail
				.SingleOrDefaultAsync(r => r.ReasId == realsId);

			var realEstateInfoDto = _mapper.Map<RealEstateInfoDto>(realEstateDetail);
			var realEstatePhotos = 
				await _context.RealEstatePhoto.Where(r => r.ReasId == realsId).ToListAsync();

			if (realEstatePhotos.Any())
			{
				realEstateInfoDto.Photos = _mapper.Map<List<RealEstatePhotoDto>>(realEstatePhotos);
			}
			else
			{
				realEstateInfoDto.Photos = new List<RealEstatePhotoDto>();
			}

			return realEstateInfoDto;
		}
	}
}
