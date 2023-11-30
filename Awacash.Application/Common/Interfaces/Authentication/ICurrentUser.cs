using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Common.Interfaces.Authentication
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUser(ClaimsPrincipal user);

        void SetCurrentUserId(string userId);
    }


    public interface ICurrentUser
    {
        string? Name { get; }

        string GetUserId();

        string? GetUserEmail();
        string? GetCustomerId();
        //string? GetFullname();
        //string? GetImageUrl();
        //string? GetCompany();

        //string? GetTenant();

        bool IsAuthenticated();

        bool IsInRole(string role);
        string? Role();
        IEnumerable<Claim>? GetUserClaims();
    }
}
