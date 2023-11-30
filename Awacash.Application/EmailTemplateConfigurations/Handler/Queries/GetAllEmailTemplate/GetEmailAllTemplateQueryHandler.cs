using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetAllEmailTemplate
{
	public class GetEmailAllTemplateQueryHandler: IRequestHandler<GetEmailAllTemplateQuery, ResponseModel<List<EmailTemplateDto>>>
	{
        private readonly IEmailConfigurationService _emailConfigurationService;
        public GetEmailAllTemplateQueryHandler(IEmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
        }

        public async Task<ResponseModel<List<EmailTemplateDto>>> Handle(GetEmailAllTemplateQuery request, CancellationToken cancellationToken)
        {
            return await _emailConfigurationService.GetAllEmailTemplateAsync();
        }
    }
}

