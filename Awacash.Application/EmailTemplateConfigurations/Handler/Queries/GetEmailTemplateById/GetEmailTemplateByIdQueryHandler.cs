using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetEmailTemplateById
{
	public class GetEmailTemplateByIdQueryHandler: IRequestHandler<GetEmailTemplateByIdQuery, ResponseModel<EmailTemplateDto>>
	{
        private readonly IEmailConfigurationService _emailConfigurationService;
        public GetEmailTemplateByIdQueryHandler(IEmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
        }

        public async Task<ResponseModel<EmailTemplateDto>> Handle(GetEmailTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _emailConfigurationService.GetEmailTemplateByIdAsync(request.Id);
        }
    }
}

