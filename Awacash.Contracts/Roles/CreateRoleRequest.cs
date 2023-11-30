using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awacash.Domain.Enums;

namespace Awacash.Contracts.Roles
{
    public record CreateRoleRequest(string Name, string Description, List<Pemission> Permission);
}
