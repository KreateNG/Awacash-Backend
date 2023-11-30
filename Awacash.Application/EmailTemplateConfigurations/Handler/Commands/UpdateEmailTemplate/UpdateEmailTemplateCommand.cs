using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Commands.UpdateEmailTemplate
{
	public record UpdateEmailTemplateCommand(string Id, string Body, string Subject):IRequest<ResponseModel<bool>>;
}

