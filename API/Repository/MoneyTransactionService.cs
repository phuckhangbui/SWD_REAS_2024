using API.DTOs;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Repository
{
    public class MoneyTransactionService : IMoneyTransactionService
    {
        private readonly IMoneyTransactionRepository _moneyTransactionRepository;
        private readonly IMoneyTransactionDetailRepository _moneyTransactionDetailRepository;

        public MoneyTransactionService(IMoneyTransactionRepository moneyTransactionRepository, IMoneyTransactionDetailRepository moneyTransactionDetailRepository)
        {
            _moneyTransactionRepository = moneyTransactionRepository;
            _moneyTransactionDetailRepository = moneyTransactionDetailRepository;
        }

        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId)
        {
            var transactionDetail = await _moneyTransactionDetailRepository.GetMoneyTransactionDetailAsync(transactionId);

            if (transactionDetail == null)
            {
                throw new BaseNotFoundException($"Transaction detail with ID {transactionId} not found.");
            }

            return transactionDetail;
        }

        public Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionParam moneyTransactionParam)
        {
            return _moneyTransactionRepository.GetMoneyTransactionsAsync(moneyTransactionParam);
        }
    }
}
