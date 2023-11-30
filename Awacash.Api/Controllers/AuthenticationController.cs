using AutoMapper;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.ChangePassword;
using Awacash.Application.Authentication.Handler.Commands.PoneVerification;
using Awacash.Application.Authentication.Handler.Commands.RefreshToken;
using Awacash.Application.Authentication.Handler.Commands.Register;
using Awacash.Application.Authentication.Handler.Commands.RegisterAccount;
using Awacash.Application.Authentication.Handler.Commands.ResetPassword;
using Awacash.Application.Authentication.Handler.Commands.SendAccountVerificationCode;
using Awacash.Application.Authentication.Handler.Commands.SendForgotPasswordVerificationCode;
using Awacash.Application.Authentication.Handler.Commands.ValidateAccount;
using Awacash.Application.Authentication.Handler.Commands.VerificationForgotPasswordCode;
using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Application.Customers.Handler.Commands.ChangePin;
using Awacash.Application.Customers.Handler.Commands.SetPin;
using Awacash.Contracts.Authentication;
using Awacash.Contracts.Customers;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class AuthController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [OpenApiOperation("Login", "Customer login to the system")]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 400)]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginQuery = new LoginQuery(request.Email, request.Password, request.DeviceId);
            var authResult = await _mediator.Send(loginQuery);

            if (authResult.IsSuccessful)
            {
                return Ok(authResult);
            }
            return BadRequest(authResult);
        }

        [AllowAnonymous]
        [OpenApiOperation("Register", "New customer registration")]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 400)]
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var registerCommand = _mapper.Map<RegisterCommand>(request); ;
            var registerResult = await _mediator.Send(registerCommand);
            if (registerResult.IsSuccessful)
            {
                return Ok(registerResult);
            }
            return BadRequest(registerResult);
        }
        [AllowAnonymous]
        [OpenApiOperation("Register", "Register with existing account")]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 400)]
        [HttpPost, Route("register-account-number")]
        public async Task<IActionResult> RegisterWithAccount(RegisterAccountRequest request)
        {
            var registerCommand = _mapper.Map<RegisterAccountCommand>(request); ;
            var registerResult = await _mediator.Send(registerCommand);
            if (registerResult.IsSuccessful)
            {
                return Ok(registerResult);
            }
            return BadRequest(registerResult);
        }


        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("set-pin")]
        public async Task<IActionResult> SetPin(SetPinRequest request)
        {
            var setPinCommand = new SetPinCommand(request.Pin);
            var response = await _mediator.Send(setPinCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("change-pin")]
        public async Task<IActionResult> ChangePin(ChangePinRequest request)
        {
            var changePinCommand = _mapper.Map<ChangePinCommand>(request);
            var response = await _mediator.Send(changePinCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var changePasswordCommand = _mapper.Map<ChangePasswordCommand>(request);
            var response = await _mediator.Send(changePasswordCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("send-phone-verification-code")]
        public async Task<IActionResult> SendPhoneNumberVerificationCode(SendPhoneNumberVerificationCodeRequest request)
        {
            var SendPhoneNumberVerificationCodeCommand = _mapper.Map<SendPhoneVerificationCodeCommand>(request);
            var response = await _mediator.Send(SendPhoneNumberVerificationCodeCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("verify-phone")]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberRequest request)
        {
            var verifyPhoneNumberCommand = _mapper.Map<VerifyPhoneCommand>(request);
            var response = await _mediator.Send(verifyPhoneNumberCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<AccountValidationResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AccountValidationResponse>), 400)]
        [HttpPost, Route("send-account-verification-code")]
        public async Task<IActionResult> SendAccountNumberVerificationCode(SendAccountNumberVerificationCodeRequest request)
        {
            var sendAccountVerificationCodeCommand = new SendAccountVerificationCodeCommand { AccountNumber = request.AccountNumber };
            var response = await _mediator.Send(sendAccountVerificationCodeCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("verify-account")]
        public async Task<IActionResult> VerifyAccount(VerifyAccountNumberRequest request)
        {
            var validateAccountCommand = new ValidateAccountCommand
            {
                Code = request.Code,
                Hash = request.Hash
            };
            var response = await _mediator.Send(validateAccountCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("send-password-verification-code")]
        public async Task<IActionResult> SendPasswordVerificationCode(SendPasswordVerificationCodeRequest request)
        {
            var sendPasswordVerificationCodeCommand = new SendForgotPasswordVerificationCodeCommand(request.Email);
            var response = await _mediator.Send(sendPasswordVerificationCodeCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("verify-forgot-password-code")]
        public async Task<IActionResult> VerifyForgotPasswordCode(VerifyForgotPasswordCodeRequest request)
        {
            var verificationForgotPasswordCodeCommand = new VerificationForgotPasswordCodeCommand(request.Code, request.Hash);
            var response = await _mediator.Send(verificationForgotPasswordCodeCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var resetPasswordCommand = new ResetPasswordCommand(request.Email, request.ConfirmPassword, request.Password, request.Hash);
            var response = await _mediator.Send(resetPasswordCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [AllowAnonymous]
        [OpenApiOperation("RefreshToken", "Customer refresh token to gain access to the system")]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AuthenticationResult>), 400)]
        [HttpPost, Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand request)
        {

            var authResult = await _mediator.Send(request);

            if (authResult.IsSuccessful)
            {
                return Ok(authResult);
            }
            return BadRequest(authResult);
        }
    }
}
