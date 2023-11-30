using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Queries.AdminLogin
{
    public class AdminLoginQueryHandler : IRequestHandler<AdminLoginQuery, ResponseModel<AdminAuthResult>>
    {
        private readonly IAuthService _authService;

        public AdminLoginQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ResponseModel<AdminAuthResult>> Handle(AdminLoginQuery request, CancellationToken cancellationToken)
        {
            return await _authService.GetAdminTokenAsync(request.Email, request.Password, "", cancellationToken);
        }
    }
}
