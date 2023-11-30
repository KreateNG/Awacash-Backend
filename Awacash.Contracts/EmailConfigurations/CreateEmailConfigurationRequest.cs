using System;
using Awacash.Domain.Enums;

namespace Awacash.Contracts.EmailConfigurations
{
	public record CreateEmailConfigurationRequest(string Body, string Subject, EmailType EmailType);
}

