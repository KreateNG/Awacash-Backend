using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetAllEmailTemplate
{
	public record GetEmailAllTemplateQuery():IRequest<ResponseModel<List<EmailTemplateDto>>>;
}
