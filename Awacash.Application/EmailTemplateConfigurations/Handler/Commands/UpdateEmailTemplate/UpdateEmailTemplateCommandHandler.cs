using System;
using Awacash.Application.EmailTemplateConfigurations.Handler.Commands.UpdateEmailTemplate;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailemplateConfigurations.Handler.Commands.UpdateEmailTemplate
{
	public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, ResponseModel<bool>>
    {
        private readonly IEmailConfigurationService _emailConfigurationService;
        public UpdateEmailTemplateCommandHandler(IEmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            return await _emailConfigurationService.UpdateEmailTemplateAsync(request.Id,request.Body, request.Subject);
        }
    }
}

