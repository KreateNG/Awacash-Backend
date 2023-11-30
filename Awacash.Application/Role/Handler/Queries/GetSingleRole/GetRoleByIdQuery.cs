using Awacash.Application.Role.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Queries.GetSingleRole
{
    public record GetRoleByIdQuery(string Id):IRequest<ResponseModel<RoleDTO>>;
}
