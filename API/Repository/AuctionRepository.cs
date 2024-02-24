using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public AuctionRepository(DataContext context, IMapper mapper) : base(context)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<PageList<AuctionDto>> GetAuctionsAsync(AuctionParam auctionParam)
		{
			var query = _context.Auction.AsQueryable();

			query = query.Where(a => a.Status != (int)AuctionStatus.NotYet);
			query = query.OrderByDescending(a => a.DateStart);

			if (!string.IsNullOrEmpty(auctionParam.Keyword))
			{
				query = query.Where(a =>
					a.RealEstate.ReasName.ToLower().Contains(auctionParam.Keyword.ToLower()) ||
					a.RealEstate.ReasAddress.ToLower().Contains(auctionParam.Keyword.ToLower()) ||
					(a.DateStart >= auctionParam.TimeStart && a.DateStart <= auctionParam.TimeEnd));
			}

			return await PageList<AuctionDto>.CreateAsync(
			query.AsNoTracking().ProjectTo<AuctionDto>(_mapper.ConfigurationProvider),
			auctionParam.PageNumber,
			auctionParam.PageSize);
		}

        public async Task<PageList<AuctionDto>> GetAuctions(AuctionParam auctionParam)
        {
            var query = _context.Auction.AsQueryable();

            //logic for search here 

            //divine the logic for 2 case with the condition or without the condition


            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<AuctionDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<AuctionDto>(_mapper.ConfigurationProvider),  //need testing for the mapper here
                auctionParam.PageNumber,
                auctionParam.PageSize);
        }

        public async Task<bool> EditAuctionStatus(string autionId, string statusCode)
        {
			try
			{
                Auction auction = await _context.Auction.FindAsync(autionId);

                if (auction == null)
                {
                    throw new Exception();
                }

                auction.Status = int.Parse(statusCode);
                bool check = await UpdateAsync(auction);
                if (check) return true;
                else return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
