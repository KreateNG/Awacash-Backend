using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Authentication.AuthGuard
{
    public class MustHavePermission : AuthorizeAttribute
    {
        public MustHavePermission(string permission)
        {
            Policy = permission;
        }
    }
}
