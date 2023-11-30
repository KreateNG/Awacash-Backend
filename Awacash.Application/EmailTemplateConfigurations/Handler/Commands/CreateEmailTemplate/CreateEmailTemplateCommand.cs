using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Commands.CreateEmailTemplate
{
	public record CreateEmailTemplateCommand(string Body, string Subject, EmailType EmailType) :IRequest<ResponseModel<EmailTemplateDto>>;
}

