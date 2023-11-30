using AutoMapper;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Handler.Commands.ChangePin;
using Awacash.Application.Customers.Handler.Commands.RegisterkMobileDevice;
using Awacash.Application.Customers.Handler.Commands.SetPin;
using Awacash.Application.Customers.Handler.Commands.UpdateCustomerNextOfKin;
using Awacash.Application.Customers.Handler.Commands.UpdateProfile;
using Awacash.Application.Customers.Handler.Commands.UploadProfile;
using Awacash.Application.Customers.Handler.Commands.ValidateBvn;
using Awacash.Application.Customers.Handler.Queries.GetAllAccounts;
using Awacash.Application.Customers.Handler.Queries.GetAllReferrals;
using Awacash.Application.Customers.Handler.Queries.GetCustomerBalance;
using Awacash.Application.Customers.Handler.Queries.GetCustomerProfile;
using Awacash.Application.Customers.Handler.Queries.GetStatement;
using Awacash.Application.Customers.Handler.Queries.InitializeCustomerBvnAuth;
using Awacash.Application.Documents.Handler.Commands.AddDocument;
using Awacash.Contracts.Customers;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{

    [Authorize]
    public class CustomersController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(ResponseModel<CustomerAccountBalanceDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("get-balance")]
        public async Task<IActionResult> GetCustomerBalanceAsync()
        {
            var GetCustomerBalanceQuery = new GetCustomerBalanceQuery();
            var response = await _mediator.Send(GetCustomerBalanceQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("profile-image")]
        public async Task<IActionResult> UpLoadProfileImageAsync(UploadProfileImageRequest uploadProfileImageRequest)
        {
            var uploadProfileCommand = new UploadProfileCommand(uploadProfileImageRequest.ProfileBase64);
            var response = await _mediator.Send(uploadProfileCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<CustomerDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<CustomerDTO>), 400)]
        [HttpGet, Route("me")]
        public async Task<IActionResult> GetCustomerProfileAsync()
        {
            var getCustomerProfileQuery = new GetCustomerProfileQuery();
            var response = await _mediator.Send(getCustomerProfileQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("initialize-bvn-auth")]
        public async Task<IActionResult> InitializeBvnAuth(InitializeCustomerBvnAuthQuery initializeCustomerBvnAuthQuery)
        {
            //var initializeCustomerBvnAuthQuery = new InitializeCustomerBvnAuthQuery();
            var response = await _mediator.Send(initializeCustomerBvnAuthQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("validate-bvn")]
        public async Task<IActionResult> ValidateBvn(ValidateCustomerBvnRequest request)
        {
            var validateBvnCommand = _mapper.Map<ValidateBvnCommand>(request);
            var response = await _mediator.Send(validateBvnCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("register-mobile")]
        public async Task<IActionResult> RegisterMobile(RegisterkMobileDeviceRequest request)
        {
            var registerkMobileDeviceCommand = _mapper.Map<RegisterkMobileDeviceCommand>(request);
            var response = await _mediator.Send(registerkMobileDeviceCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("generate-otp")]
        public async Task<IActionResult> GenerateOtp(RegisterkMobileDeviceRequest request)
        {
            var registerkMobileDeviceCommand = _mapper.Map<RegisterkMobileDeviceCommand>(request);
            var response = await _mediator.Send(registerkMobileDeviceCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("next-of-kin")]
        public async Task<IActionResult> UpdateCustomerNextOfKinAsync(UpdateCustomerNextOfKinRequest Payload)
        {
            var updateCustomerNextOfKinCommand = new UpdateCustomerNextOfKinCommand
            {
                Address = Payload.Address,
                State = Payload.State,
                City = Payload.City,
                Country = Payload.Country,
                NextOfKinName = Payload.NextOfKinName,
                NextOfKinPhoneNumber = Payload.NextOfKinPhoneNumber,
                NextOfKinRelationship = Payload.NextOfKinRelationship,
                NextOfKinEmail = Payload.NextOfKinEmail,
                NextOfKinAddress = Payload.NextOfKinAddress
            };
            var response = await _mediator.Send(updateCustomerNextOfKinCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("upgrade-account-doc")]
        public async Task<IActionResult> UpadateKycDocsAsync(UpadateKycDocsRequest request)
        {

            var response = await _mediator.Send(new AddDocumentCommand(request.IDBase64, request.UtilityBase64, request.IDNumber, request.IDType));
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<AccountDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AccountDTO>), 400)]
        [HttpGet, Route("accounts")]
        public async Task<IActionResult> GetCustomerAccountsAsync()
        {
            var getAllAccountsQuery = new GetAllAccountsQuery();
            var response = await _mediator.Send(getAllAccountsQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("statement/{account}/{from}/{to}")]
        public async Task<IActionResult> GenerateStatement(string account, DateTime from, DateTime to)
        {
            var getStatementQuery = new GetStatementQuery(account, from, to);
            var response = await _mediator.Send(getStatementQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<CustomerDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPut, Route("update-profile")]
        public async Task<IActionResult> UpdateProfle(UpdateCustomerProfileCommand updateCustomerProfileCommand)
        {
            var response = await _mediator.Send(updateCustomerProfileCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<ReferralDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpGet, Route("referrees")]
        public async Task<IActionResult> GetReferral()
        {
            var response = await _mediator.Send(new GetAllReferralsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
