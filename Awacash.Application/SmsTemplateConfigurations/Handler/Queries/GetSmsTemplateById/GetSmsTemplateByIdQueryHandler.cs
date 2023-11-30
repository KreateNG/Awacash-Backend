using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetSmsTemplateById
{
	public class GetSmsTemplateByIdQueryHandler: IRequestHandler<GetSmsTemplateByIdQuery, ResponseModel<SmsTemplateDto>>
	{
        private readonly ISmsConfigurationService _smsConfigurationService;
        public GetSmsTemplateByIdQueryHandler(ISmsConfigurationService smsConfigurationService)
        {
            _smsConfigurationService = smsConfigurationService;
        }

        public async Task<ResponseModel<SmsTemplateDto>> Handle(GetSmsTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _smsConfigurationService.GetSmsTemplateByIdAsync(request.Id);
        }
    }
}

