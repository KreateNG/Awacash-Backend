using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.IdentityModel
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName, string? description = null)
            : base(roleName)
        {
            Description = description;
            NormalizedName = roleName.ToUpperInvariant();
        }
    }
}
