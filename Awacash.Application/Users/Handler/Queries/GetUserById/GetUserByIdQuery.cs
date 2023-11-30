using Awacash.Application.Users.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Queries.GetUserById
{
    public record GetUserByIdQuery(string Id):IRequest<ResponseModel<UserDTO>>;
}
