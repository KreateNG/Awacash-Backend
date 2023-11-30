using System;
using Awacash.Domain.Enums;

namespace Awacash.Contracts.SmsConfigurations
{
	public record CreateSmsConfigurationRequest(string Message, SmsType SmsType);
}

