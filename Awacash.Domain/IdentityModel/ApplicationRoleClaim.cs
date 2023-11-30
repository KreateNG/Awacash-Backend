using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.IdentityModel
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string ClaimId { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public ApplicationRoleClaim()
        {
        }
    }
}
