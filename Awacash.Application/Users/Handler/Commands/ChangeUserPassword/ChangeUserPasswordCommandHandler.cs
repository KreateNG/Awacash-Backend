using System;
using Awacash.Application.Users.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Users.Handler.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, ResponseModel<string>>
    {
        private readonly IUserService _userService;

        public ChangeUserPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseModel<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
        }
    }
}

