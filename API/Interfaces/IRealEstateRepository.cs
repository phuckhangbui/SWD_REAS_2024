using API.Entity;

namespace API.Interfaces
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<List<RealEstate>> GetRealEstatesAsync();
        Task<RealEstate> GetRealEstateAsync(int reasId);
		Task<RealEstate> UpdateRealEstateAsync(RealEstate realEstate);
        Task<RealEstate> GetRealEstateWithStatusAsync(int reasId, int status);
	}
}
