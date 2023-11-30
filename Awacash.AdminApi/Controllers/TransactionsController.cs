using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Handler.Queries.GetSingleCustomer;
using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Handlers.Queries.GetPaginatedTransactions;
using Awacash.Application.Transactions.Handlers.Queries.GetTotalSystemBalance;
using Awacash.Application.Transactions.Handlers.Queries.GetTransactionById;
using Awacash.Application.Transactions.Handlers.Queries.TransactionStatus;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class TransactionsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<TransactionDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<TransactionDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPagedTransactionsAsync([FromQuery] GetPaginatedTransactionQuery filter)
        {
            var response = await _mediator.Send(filter);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<TransactionDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<TransactionDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetTransactionById(string id)
        {
            var getTransactionByIdQuery = new GetTransactionByIdQuery(id);
            var response = await _mediator.Send(getTransactionByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<TransactionQuery>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<TransactionQuery>>), 400)]
        [HttpGet, Route("trans-query/{accountNumber}")]
        public async Task<IActionResult> transQuery(string accountNumber)
        {
            var transactionStatusQuery = new TransactionStatusQuery(accountNumber);
            var response = await _mediator.Send(transactionStatusQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<WalletBalanceDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<WalletBalanceDto>), 400)]
        [HttpGet, Route("collection-balance")]
        public async Task<IActionResult> GetCollectionBalance()
        {
            var getTotalSystemBalanceQuery = new GetTotalSystemBalanceQuery();
            var response = await _mediator.Send(getTotalSystemBalanceQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
