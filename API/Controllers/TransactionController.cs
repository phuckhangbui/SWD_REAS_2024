using API.Errors;
using API.Extension;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly IMoneyTransactionRepository _transactionRepository;
        private readonly IMoneyTransactionDetailRepository _transactionDetailRepository;

        private const string BaseUri = "/api/transactions";
        private const string DetailUri = BaseUri + "/{transactionId}";

        public TransactionController(IMoneyTransactionRepository transactionRepository,
            IMoneyTransactionDetailRepository moneyTransactionDetailRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = moneyTransactionDetailRepository;
        }

        [HttpGet(BaseUri)]
        public async Task<IActionResult> GetTransactionHistory([FromQuery] MoneyTransactionParam moneyTransactionParam)
        {
            var transactionsHistory = await _transactionRepository.GetMoneyTransactionsAsync(moneyTransactionParam);
                
            Response.AddPaginationHeader(new PaginationHeader(transactionsHistory.CurrentPage, transactionsHistory.PageSize,
            transactionsHistory.TotalCount, transactionsHistory.TotalPages));

            return Ok(transactionsHistory);
        }

        [HttpGet(DetailUri)]
        public async Task<IActionResult> GetTransactionHistoryDetail(int transactionId)
        {
            var transactionDetail = await _transactionDetailRepository.GetMoneyTransactionDetailAsync(transactionId);
            if (transactionDetail == null)
            {
                return BadRequest(new ApiResponse(404, "Transaction history detail not found"));
            }

            return Ok(transactionDetail);
        }
    }
}
