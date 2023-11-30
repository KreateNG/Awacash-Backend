using Awacash.Application.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Common
{
    public record AdminAuthResult(UserDTO? User, string? Token);
}
