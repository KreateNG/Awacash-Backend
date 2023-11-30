using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string id, string firstName, string lastName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
