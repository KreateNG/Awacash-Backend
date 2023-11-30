using System;
using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;

namespace Awacash.Application.SmsTemplateConfigurations.DTOs
{
	public class SmsTemplateDto:BaseDTO
	{
        public string? Message { get; set; }
        public SmsType SmsType { get; set; }
    }
}

