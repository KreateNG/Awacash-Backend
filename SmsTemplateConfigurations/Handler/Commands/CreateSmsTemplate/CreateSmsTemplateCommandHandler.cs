using System;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Commands.CreateSmsTemplate
{
    public class CreateSmsTemplateCommandHandler : IRequestHandler<CreateSmsTemplateCommand, ResponseModel<SmsTemplateDto>>
    {
        private readonly ISmsConfigurationService _smsConfigurationService;
        public CreateSmsTemplateCommandHandler(ISmsConfigurationService smsConfigurationService)
        {
            _smsConfigurationService = smsConfigurationService;
        }
        public async Task<ResponseModel<SmsTemplateDto>> Handle(CreateSmsTemplateCommand request, CancellationToken cancellationToken)
        {
            return await _smsConfigurationService.CreateSmsTemplateAsync(request.Message, request.SmsType);
        }
    }
}

