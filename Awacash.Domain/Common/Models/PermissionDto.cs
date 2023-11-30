using System;
using Awacash.Domain.Enums;
using System.Security.Cryptography.X509Certificates;

namespace Awacash.Domain.Common.Models
{
	public class PermissionDto
	{
        public PermissionDto(string groupName, string name, string description, Pemission permission)
        {
            Permission = permission;
            GroupName = groupName;
            ShortName = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public string GroupName { get; private set; }
        
        public string ShortName { get; private set; }
        
        public string Description { get; private set; }
        
        public Pemission Permission { get; private set; }
    }
}

