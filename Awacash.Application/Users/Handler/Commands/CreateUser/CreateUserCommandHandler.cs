using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel<UserDTO>>
    {
        private IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseModel<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.RoleId);
        }
    }
}
