using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using Microsoft.AspNetCore.Mvc;

namespace API.Interface.Service
{
    public interface IMemberDepositAmountService : IBaseService<DepositAmount>
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMember(int userMember);
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMemberWhenSearch(SearchDepositAmountParam searchDepositAmountParam, int userMember);
    }
}
