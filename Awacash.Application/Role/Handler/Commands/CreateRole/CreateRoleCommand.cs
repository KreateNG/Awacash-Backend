using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Commands.CreateRole
{
    public record CreateRoleCommand(string Name, string Description, List<Pemission> Permission) : IRequest<ResponseModel<bool>>;
}
