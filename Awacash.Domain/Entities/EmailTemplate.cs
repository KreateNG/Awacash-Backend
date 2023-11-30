using System;
using Awacash.Domain.Enums;

namespace Awacash.Domain.Entities
{
	public class EmailTemplate: BaseEntity
    {
		public string? Subject { get; set; }
		public string? Body { get; set; }
		public EmailType EmailType { get; set; }
	}
}

