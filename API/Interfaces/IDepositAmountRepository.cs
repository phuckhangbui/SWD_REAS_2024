using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interfaces
{
    public interface IDepositAmountRepository : IBaseRepository<DepositAmount>
    {
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMember(int id);
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMemberBySearch(SearchDepositAmountDto searchDepositAmountDto, int id);
    }
}
