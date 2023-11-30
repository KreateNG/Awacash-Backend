using Awacash.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.FilterModels
{
    public class RoleFilterModel: BaseQueryModel
    {
        public string? Name { get; set; }
    }
}
