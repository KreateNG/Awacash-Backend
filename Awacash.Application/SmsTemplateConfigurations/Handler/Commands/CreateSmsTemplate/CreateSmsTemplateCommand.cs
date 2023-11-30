using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Commands.CreateSmsTemplate
{
	public record CreateSmsTemplateCommand(string Message, SmsType SmsType):IRequest<ResponseModel<SmsTemplateDto>>;
}

