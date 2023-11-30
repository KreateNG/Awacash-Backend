using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Commands.UpdateSmsTemplate
{
	public record UpdateSmsTemplateCommand(string Id, string Message):IRequest<ResponseModel<bool>>;
}

