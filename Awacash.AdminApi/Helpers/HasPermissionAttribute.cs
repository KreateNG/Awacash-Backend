using System;
using Awacash.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Awacash.AdminApi.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Pemission permission) : base(permission.ToString())
        { }
    }
}

