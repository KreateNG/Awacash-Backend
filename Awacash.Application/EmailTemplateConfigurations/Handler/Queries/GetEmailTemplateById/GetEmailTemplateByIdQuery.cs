using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetEmailTemplateById
{
	public record GetEmailTemplateByIdQuery(string Id):IRequest<ResponseModel<EmailTemplateDto>>;
}

