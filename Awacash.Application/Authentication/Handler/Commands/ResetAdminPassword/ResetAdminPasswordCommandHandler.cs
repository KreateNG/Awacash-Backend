using System;
using Awacash.Application.Authentication.Handler.Commands.ResetPassword;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.ResetAdminPassword
{
	public class ResetAdminPasswordCommandHandler: IRequestHandler<ResetAdminPasswordCommand, ResponseModel<bool>>
    {
        private readonly IAuthService _authService;
        public ResetAdminPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        
        public async Task<ResponseModel<bool>> Handle(ResetAdminPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResetUserPassword(request.Email, request.ConfirmPasswprd, request.Password);
        }
    }
}

