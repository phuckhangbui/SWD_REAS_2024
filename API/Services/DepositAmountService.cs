using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
{
    public class DepositAmountService : IDepositAmountService
    {
        private readonly IDepositAmountRepository _depositAmountRepository;

        public DepositAmountService(IDepositAmountRepository depositAmountRepository)
        {
            _depositAmountRepository = depositAmountRepository;
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmounts(DepositAmountParam depositAmountParam)
        {
            return await _depositAmountRepository.GetDepositAmountsAsync(depositAmountParam);
        }


    }
}
