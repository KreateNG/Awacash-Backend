using System.Security.Principal;
using AutoMapper;
using Awacash.Application.Loans.Handler.Commands;
using Awacash.Application.Loans.Handler.Queries;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Handler.Queries.GetAllSavingsById;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class LoansController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LoansController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("loan-request")]
        public async Task<IActionResult> RequestLoanAsync(CreateLoanCommand request)
        {
            var response = await _mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("loan-repayment")]
        public async Task<IActionResult> LoanRepaymentAsync(LoanRepaymentCommand request)
        {
            var response = await _mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<List<LoanStatusModel>>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("{status}")]
        public async Task<IActionResult> GetLoanByStatusAsync(string status)
        {
            var getLoanByStatusQuerry = new GetLoanByStatusQuerry(status);
            var response = await _mediator.Send(getLoanByStatusQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<LoanModel>>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetLoansAsync()
        {
            var getLoanQuerry = new GetLoanQuerry();
            var response = await _mediator.Send(getLoanQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<LoanBalanceModel>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 400)]
        [HttpGet, Route("balance")]
        public async Task<IActionResult> GetLoansBalanceAsync()
        {
            var getCustomerLoanBalanceQuery = new GetCustomerLoanBalanceQuery();
            var response = await _mediator.Send(getCustomerLoanBalanceQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<LoanRepaymentModel>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("repayment/{account}")]
        public async Task<IActionResult> GetLoansTotayRepaymentAsync(string account)
        {
            var getCustomerLoanRepaymentQuery = new GetCustomerLoanRepaymentQuery(account);
            var response = await _mediator.Send(getCustomerLoanRepaymentQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}

