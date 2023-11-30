using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Commands.UpdateRole
{
    public record UpdateRoleCommand(string RoleId, string Name, string Description, List<int> Permissions) : IRequest<ResponseModel<bool>>;
}
