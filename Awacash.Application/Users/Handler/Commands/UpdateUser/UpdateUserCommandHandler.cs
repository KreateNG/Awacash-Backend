using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseModel<UserDTO>>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseModel<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserAsync(request.Id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.UserName, request.RoleId);
        }
    }
}
