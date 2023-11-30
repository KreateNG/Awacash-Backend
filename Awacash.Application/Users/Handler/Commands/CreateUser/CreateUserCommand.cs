using Awacash.Application.Users.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Commands.CreateUser
{
    public record CreateUserCommand(string FirstName, string LastName, string Email, string PhoneNumber, string RoleId) : IRequest<ResponseModel<UserDTO>>;
}
