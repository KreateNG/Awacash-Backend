using System;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.ValidateAccount
{
    public class ValidateAccountCommandHandler : IRequestHandler<ValidateAccountCommand, ResponseModel<string>>
    {
        private readonly IAuthService _authService;
        public ValidateAccountCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseModel<string>> Handle(ValidateAccountCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ValidateAccountNumber(request.Code, request.Hash);
        }
    }
}

