using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResponseModel<bool>>
    {
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResponseModel<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ResetPassword(request.Email, request.ConfirmPasswprd, request.Password, request.Hash);
        }
    }
}
