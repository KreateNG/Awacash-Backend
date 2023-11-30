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

namespace Awacash.Application.Authentication.Handler.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ResponseModel<AuthenticationResult>>
    {
        private readonly IAuthService _authService;

        public LoginQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseModel<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            return await _authService.GetTokenAsync(query.Email, query.Password, query.DeviceId, "", cancellationToken);
        }
    }
}
