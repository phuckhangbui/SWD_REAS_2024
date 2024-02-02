using API.Data;
using API.Entity;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstateRepository : BaseRepository<RealEstate>, IRealEstateRepository
    {

        private readonly DataContext _context;

        public RealEstateRepository(DataContext context) : base(context)
        {
            _context = context;
        }

		public async Task<RealEstate> GetRealEstateAsync(int reasId)
		    => await _context.RealEstate.SingleOrDefaultAsync(r => r.ReasId == reasId);

		public async Task<List<RealEstate>> GetRealEstatesAsync()
			=> await _context.RealEstate.ToListAsync();

		public async Task<RealEstate> GetRealEstateWithStatusAsync(int reasId, int status)
			=> await _context.RealEstate.SingleOrDefaultAsync(r => r.ReasId == reasId && r.ReasStatus == status);

		public async Task<RealEstate> UpdateRealEstateAsync(RealEstate realEstate)
		{
			_context.Entry(realEstate).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return realEstate;
		}
	}
}
