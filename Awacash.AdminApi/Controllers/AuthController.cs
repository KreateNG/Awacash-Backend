using AutoMapper;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.AdminForgotPassword;
using Awacash.Application.Authentication.Handler.Commands.PoneVerification;
using Awacash.Application.Authentication.Handler.Commands.ResetAdminPassword;
using Awacash.Application.Authentication.Handler.Commands.ResetPassword;
using Awacash.Application.Authentication.Handler.Commands.SendForgotPasswordVerificationCode;
using Awacash.Application.Authentication.Handler.Commands.VerificationForgotPasswordCode;
using Awacash.Application.Authentication.Handler.Queries.AdminLogin;
using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Contracts.Authentication;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Awacash.AdminApi.Controllers
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
        [OpenApiOperation("Login", "User login to the system")]
        [ProducesResponseType(typeof(ResponseModel<AdminAuthResult>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AdminAuthResult>), 400)]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(AdminLoginRequest request)
        {
            var loginQuery = new AdminLoginQuery(request.Email, request.Password);
            var authResult = await _mediator.Send(loginQuery);

            if (authResult.IsSuccessful)
            {
                return Ok(authResult);
            }
            return BadRequest(authResult);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<string>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        [HttpPost, Route("forgot-password")]
        public async Task<IActionResult> SendPasswordVerificationCode(SendPasswordVerificationCodeRequest request)
        {
            var adminForgotPasswordCommand = new AdminForgotPasswordCommand(request.Email);
            var response = await _mediator.Send(adminForgotPasswordCommand);

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
            var resetAdminPasswordCommand = new ResetAdminPasswordCommand(request.Email, request.ConfirmPassword, request.Password);
            var response = await _mediator.Send(resetAdminPasswordCommand);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
