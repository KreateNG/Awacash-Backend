using System;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.SendForgotPasswordVerificationCode;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.SendAccountVerificationCode
{
    public class SendAccountVerificationCodeCommandHandler : IRequestHandler<SendAccountVerificationCodeCommand, ResponseModel<AccountValidationResponse>>
    {
        private readonly IAuthService _authService;
        public SendAccountVerificationCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseModel<AccountValidationResponse>> Handle(SendAccountVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SendAccountNumberVerificationCode(request.AccountNumber);
        }
    }
}

