using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetAllSmsTemplate
{
	public record GetSmsAllTemplateQuery():IRequest<ResponseModel<List<SmsTemplateDto>>>;
}

