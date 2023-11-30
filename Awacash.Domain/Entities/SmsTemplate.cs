using System;
using Awacash.Domain.Enums;

namespace Awacash.Domain.Entities
{
	public class SmsTemplate: BaseEntity
    {
		public string? Message { get; set; }
		public SmsType SmsType { get; set; }
	}
}

