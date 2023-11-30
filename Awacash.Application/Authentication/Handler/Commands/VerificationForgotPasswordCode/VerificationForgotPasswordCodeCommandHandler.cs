using Awacash.Application.Authentication.Handler.Commands.PoneVerification;
using Awacash.Application.Authentication.Services;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.VerificationForgotPasswordCode
{
    public class VerificationForgotPasswordCodeCommandHandler : IRequestHandler<VerificationForgotPasswordCodeCommand, ResponseModel<string>>
    {
        private readonly IAuthService _authService;

        public VerificationForgotPasswordCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<ResponseModel<string>> Handle(VerificationForgotPasswordCodeCommand request, CancellationToken cancellationToken)
        {
            return _authService.VerifyForgotPasswordCode(request.Code, request.Hash);
        }
    }
}
