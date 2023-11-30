using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseModel<AuthenticationResult>>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResponseModel<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterCustomer(request.Email, request.Password, request.Pin, request.FirstName, request.LastName, request.MiddleName, request.PhoneNumber, request.Hash, request.ReferralCode, "::1", cancellationToken);
        }
    }
}
