using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awacash.Domain.Common.Models;

namespace Awacash.Application.Role.DTOs
{
    public record RoleDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<PermissionDto>? Pemissions { get; set; } = new List<PermissionDto>();
    }
}
