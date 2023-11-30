using AutoMapper;
using Awacash.Application.Customers.Handler.Queries.GetAllCustomers;
using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.FilterModels;
using Awacash.Application.Transactions.Handlers.Queries.GetCoreTransaction;
using Awacash.Application.Transactions.Handlers.Queries.GetCustomerPaginatedTransactions;
using Awacash.Application.Transactions.Handlers.Queries.GetCustomerTransactions;
using Awacash.Application.Transactions.Handlers.Queries.GetTransactionById;
using Awacash.Application.Wema.Handler.Queries.AccountLookUp;
using Awacash.Contracts.Transactions;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class TransactionsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TransactionsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }


        #region commands
        //[ProducesResponseType(typeof(ResponseModel), 200)]
        //[ProducesResponseType(typeof(ResponseModel), 400)]
        //[HttpPost, Route("local-transfer")]
        //public async Task<IActionResult> PostLocalTransferAsync(LocalTransferRequest request)
        //{
        //    var localTransferCommand = _mapper.Map<LocalTransferCommand>(request);
        //    var response = await _mediator.Send(localTransferCommand);
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}

        //[ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 200)]
        //[ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 400)]
        //[HttpPost, Route("inter-bank-transfer")]
        //public async Task<IActionResult> PostInterBankTransferAsync(InterBankTransferRequest request)
        //{
        //    var interBankTransferCommand = _mapper.Map<InterBankTransferCommand>(request);
        //    var response = await _mediator.Send(interBankTransferCommand);
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}


        //[ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 200)]
        //[ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 400)]
        //[HttpPost, Route("inter-bank-name-enquiry")]
        //public async Task<IActionResult> PostNipNameEnquiryAsync(NipNameEnquiryRequest request)
        //{
        //    var nipNameEnquiryCommand = _mapper.Map<NipNameEnquiryCommand>(request);
        //    var response = await _mediator.Send(nipNameEnquiryCommand);
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}
        #endregion


        #region Queries


        [ProducesResponseType(typeof(ResponseModel<PagedResult<TransactionDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<TransactionDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetCustomerPagedTransactions([FromQuery] GetCustomerPaginatedTransactionQuery getCustomerPaginatedTransactionQuery)
        {
            var response = await _mediator.Send(getCustomerPaginatedTransactionQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<List<TransactionDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<TransactionDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetCustomerTransactions()
        {
            var getCustomerTransactionsQuery = new GetCustomerTransactionsQuery();
            var response = await _mediator.Send(getCustomerTransactionsQuery);
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


        [ProducesResponseType(typeof(ResponseModel<List<TransactionResponseDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("fetch")]
        public async Task<IActionResult> GetTransactionCore([FromQuery] GetCoreTransactionQuerry model)
        {
            //DateTime? startDate = model.StartDate;
            //DateTime? endDate = model.EndDate;
            //if (!model.StartDate.HasValue)
            //{
            //    startDate = DateTime.UtcNow.AddMonths(-1);
            //}
            //if (!model.EndDate.HasValue)
            //{
            //    endDate = DateTime.UtcNow;
            //}
            //var getCoreTransactionQuerry = new GetCoreTransactionQuerry { AccountNumber = account, StartDate = startDate, EndDate = endDate };
            var response = await _mediator.Send(model);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        #endregion
    }
}
