using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface IDepositAmountService
    {
        Task<PageList<DepositAmountDto>> GetDepositAmounts(DepositAmountParam depositAmountParam);

        Task<DepositAmountDto> CreateDepositAmount(int customerId, int reasId);

        Task<DepositAmountDto> UpdateStatusToDeposited(int customerId, int reasId, DateTime paymentTime);

        DepositAmount GetDepositAmount(int customerId, int reasId);
    }
}
