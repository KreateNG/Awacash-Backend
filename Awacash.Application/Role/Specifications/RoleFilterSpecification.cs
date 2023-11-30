using Awacash.Application.Role.FilterModels;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Specifications
{
    public class RoleFilterSpecification : BaseSpecification<ApplicationRole>
    {
        public RoleFilterSpecification(string name) : 
            base(
                f =>
                (string.IsNullOrWhiteSpace(name) || f.Name.ToLower() == name.ToLower())
            )
        {
        }
    }
}
