using System;
using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;

namespace Awacash.Application.EmailTemplateConfigurations.DTOs
{
	public class EmailTemplateDto:BaseDTO
	{
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public EmailType EmailType { get; set; }
    }
}

