using Awacash.Application.Users.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Commands.UpdateUser
{
    public record UpdateUserCommand(string Id, string FirstName, string LastName, string Email, string PhoneNumber, string UserName, string RoleId) : IRequest<ResponseModel<UserDTO>>;
}
