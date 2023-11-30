using Awacash.Application.Users.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Queries.GetAllUsers
{
    public record GetAllUsersQuery():IRequest<ResponseModel<List<UserDTO>>>;
}
