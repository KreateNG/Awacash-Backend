using System;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Handler.Commands.CreateEmailTemplate;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.EmailTemplateConfigurations.Handler.Commands.CreateEmailTemplate
{
    public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, ResponseModel<EmailTemplateDto>>
    {
        private readonly IEmailConfigurationService _emailConfigurationService;
        public CreateEmailTemplateCommandHandler(IEmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
        }
        public async Task<ResponseModel<EmailTemplateDto>> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            return await _emailConfigurationService.CreateEmailTemplateAsync(request.Body, request.Subject, request.EmailType);
        }
    }
}

