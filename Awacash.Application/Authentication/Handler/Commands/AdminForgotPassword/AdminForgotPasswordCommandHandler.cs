using System;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.AdminForgotPassword
{
    public class AdminForgotPasswordCommandHandler : IRequestHandler<AdminForgotPasswordCommand, ResponseModel<string>>
    {
        private readonly IAuthService _authService;
        public async Task<ResponseModel<string>> Handle(AdminForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SendUserForgotPasswordVerificationCode(request.Email, "::1", cancellationToken);
        }
    }
}

