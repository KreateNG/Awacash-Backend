using Awacash.Application.Authentication.Common.Extentions;
using Awacash.Application.Common.Interfaces.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Authentication
{
    public class CurrentUser : ICurrentUser, ICurrentUserInitializer
    {
        private ClaimsPrincipal? _user;

        public string? Name => _user?.Identity?.Name;

        private string _userId = Guid.Empty.ToString();

        public string GetUserId() =>
            IsAuthenticated()
                ? _user?.GetUserId() ?? Guid.Empty.ToString()
                : _userId;

        public string? GetCustomerId() =>
            IsAuthenticated()
                ? _user!.GetCustomerId()
                : string.Empty;


        public string? GetUserEmail() =>
            IsAuthenticated()
                ? _user!.GetEmail()
                : string.Empty;

        //public string? GetImageUrl() =>
        //    IsAuthenticated()
        //        ? _user!.GetImageUrl()
        //        : string.Empty;

        //public string? GetCompany() =>
        //    IsAuthenticated()
        //        ? _user!.GetCompany()
        //        : string.Empty;

        //public string? GetFullname() =>
        //    IsAuthenticated()
        //        ? _user!.GetFullName()
        //        : string.Empty;

        public bool IsAuthenticated() =>
            _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) =>
            _user?.IsInRole(role) is true;

        public string? Role() =>
            IsAuthenticated()
                ? _user!.Role()
                : string.Empty;

        public IEnumerable<Claim>? GetUserClaims() =>
            _user?.Claims;

        //public string? GetTenant() =>
        //    IsAuthenticated() ? _user?.GetTenant() : string.Empty;

        public void SetCurrentUser(ClaimsPrincipal user)
        {
            if (_user != null)
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            _user = user;
        }

        public void SetCurrentUserId(string userId)
        {
            if (_userId != Guid.Empty.ToString())
            {
                throw new Exception("Method reserved for in-scope initialization");
            }

            if (!string.IsNullOrEmpty(userId))
            {
                _userId = userId;
            }
        }
    }
}
