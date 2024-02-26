using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface IDepositAmountService
    {
        Task<PageList<DepositAmountDto>> GetDepositAmounts(DepositAmountParam depositAmountParam);
    }
}
