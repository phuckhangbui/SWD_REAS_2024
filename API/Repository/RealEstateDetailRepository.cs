using API.Data;
using API.Entity;
using API.Interfaces;
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

		public Task<RealEstateDetail> GetRealEstateDetailAsync(int realsId)
			=> _context.RealEstateDetail.SingleOrDefaultAsync(r => r.ReasId == realsId);
	}
}
