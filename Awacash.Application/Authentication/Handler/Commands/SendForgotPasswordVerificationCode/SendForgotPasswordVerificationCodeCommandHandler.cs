using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.Register;
using Awacash.Application.Authentication.Services;
using Awacash.Application.Customers.Handler.Commands.ChangePin;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.SendForgotPasswordVerificationCode
{
    public class SendForgotPasswordVerificationCodeCommandHandler : IRequestHandler<SendForgotPasswordVerificationCodeCommand, ResponseModel<string>>
    {
        private readonly IAuthService _authService;

        public SendForgotPasswordVerificationCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResponseModel<string>> Handle(SendForgotPasswordVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SendForgotPasswordVerificationCode(request.Email, "::1", cancellationToken);
        }
    }
}
