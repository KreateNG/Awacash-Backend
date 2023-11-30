using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetAllSmsTemplate
{
	public class GetSmsAllTemplateQueryHandler: IRequestHandler<GetSmsAllTemplateQuery, ResponseModel<List<SmsTemplateDto>>>
	{
        private readonly ISmsConfigurationService _smsConfigurationService;
        public GetSmsAllTemplateQueryHandler(ISmsConfigurationService smsConfigurationService)
        {
            _smsConfigurationService = smsConfigurationService;
        }

        public async Task<ResponseModel<List<SmsTemplateDto>>> Handle(GetSmsAllTemplateQuery request, CancellationToken cancellationToken)
        {
            return await _smsConfigurationService.GetAllSmsTemplateAsync();
        }
    }
}

