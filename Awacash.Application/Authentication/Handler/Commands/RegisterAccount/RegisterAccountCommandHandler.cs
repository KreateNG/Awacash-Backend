using System;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.RegisterAccount
{
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, ResponseModel<AuthenticationResult>>
    {
        private readonly IAuthService _authService;
        public RegisterAccountCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResponseModel<AuthenticationResult>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterCustomerWithAccount(request.Email, request.Password, request.Pin, request.FirstName, request.LastName, request.MiddleName, request.PhoneNumber, request.AccountId, request.Hash, "::1", cancellationToken);
        }
    }
}

