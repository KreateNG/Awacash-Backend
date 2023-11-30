using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Roles
{
    public record UpdateRoleRequest(string Name, string Description, List<int> Permission);
}
