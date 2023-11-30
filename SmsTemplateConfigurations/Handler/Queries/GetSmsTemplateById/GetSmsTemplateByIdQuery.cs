using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetSmsTemplateById
{
	public record GetSmsTemplateByIdQuery(string Id):IRequest<ResponseModel<SmsTemplateDto>>;
}

