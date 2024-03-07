﻿using API.Errors;
using API.Exceptions;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MoneyTransactionController : BaseApiController
    {
        private readonly IMoneyTransactionService _moneyTransactionService;

        public MoneyTransactionController(IMoneyTransactionService moneyTransactionService)
        {
            _moneyTransactionService = moneyTransactionService;
        }

        private const string BaseUri = "/api/transactions";
        private const string DetailUri = BaseUri + "/{transactionId}";

        [HttpPost(BaseUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetTransactionHistory([FromBody] MoneyTransactionRequest moneyTransactionRequest)
        {
            try
            {
                var transactionsHistory = await _moneyTransactionService.GetMoneyTransactions(moneyTransactionRequest);

                Response.AddPaginationHeader(new PaginationHeader(transactionsHistory.CurrentPage, transactionsHistory.PageSize,
                transactionsHistory.TotalCount, transactionsHistory.TotalPages));

                return Ok(transactionsHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(DetailUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetTransactionHistoryDetail(int transactionId)
        {
            try
            {
                var transactionDetail = await _moneyTransactionService.GetMoneyTransactionDetail(transactionId);

                return Ok(transactionDetail);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
