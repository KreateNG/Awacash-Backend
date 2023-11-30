using AutoMapper;
using Awacash.Application.Customers.Services;
using Awacash.Application.Transactions.Handlers.Commands.AccountTransactionNotification;
using Awacash.Application.Wema.Handler.Queries.AccountLookUp;
using Awacash.Contracts.Customers;
using Awacash.Contracts.Transactions;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{

    [Authorize]
    public class WemaController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly IAwacashThirdPartyService _berachahThirdPartyService;
        private readonly ICustomerService _customerService;
        public WemaController(IMapper mapper, IMediator mediator, IBankOneAccountService bankOneAccountService, IAwacashThirdPartyService berachahThirdPartyService, ICustomerService customerService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankOneAccountService = bankOneAccountService;
            _berachahThirdPartyService = berachahThirdPartyService;
            _customerService = customerService;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("account-lookup/{AccountNumber}")]
        public async Task<IActionResult> WemaAccountLookAsync(string AccountNumber)
        {
            var accountLookUpQuerry = new AccountLookUpQuerry(AccountNumber);
            var response = await _mediator.Send(accountLookUpQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("account-notificaation-transaction")]
        public async Task<IActionResult> WemaAccountNotificationTransactionAsync(AccountTransactionNotificationRequest request)
        {
            var accountTransactionNotificationCommand = _mapper.Map<AccountTransactionNotificationCommad>(request);
            var response = await _mediator.Send(accountTransactionNotificationCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 400)]
        [HttpGet, Route("open-account")]
        public async Task<IActionResult> Test()
        {
            var res = await _bankOneAccountService.AccountOpening("Moses", "Obika", "Chukwuma", "", "09035654345", "male", "Lagos", "1985-01-01", "", "", "test@mosestest.com", "", "", "", "");

            return BadRequest(res);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 400)]
        [HttpGet, Route("test")]
        public async Task<IActionResult> TestS()
        {
            await _berachahThirdPartyService.SendSms("08064318298", "test", "4545345656", "23454");
            var res = await _bankOneAccountService.AccountOpening("Moses", "Obika", "Chukwuma", "", "09035654345", "male", "Lagos", "1985-01-01", "", "", "test@mosestest.com", "", "", "", "");

            return BadRequest(res);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<NameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("test-date")]
        public async Task<IActionResult> DateTest(ValidateCustomerBvnRequest request)
        {
            var check = await _customerService.TestCustomerBvn(request.FirstName, request.LastName, request.DateOfBirth, request.AccessToken);
            return Ok(check);
        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<int>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("test-referral")]
        public async Task<IActionResult> TestReferral()
        {
            var check = await _customerService.ProcessRefeerral();
            return Ok(check);
        }
    }
}
