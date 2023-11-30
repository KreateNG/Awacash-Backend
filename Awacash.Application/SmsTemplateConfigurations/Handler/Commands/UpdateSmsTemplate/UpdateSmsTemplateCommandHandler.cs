using System;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.SmsTemplateConfigurations.Handler.Commands.UpdateSmsTemplate
{
	public class UpdateSmsTemplateCommandHandler : IRequestHandler<UpdateSmsTemplateCommand, ResponseModel<bool>>
    {
        private readonly ISmsConfigurationService _smsConfigurationService;
        public UpdateSmsTemplateCommandHandler(ISmsConfigurationService smsConfigurationService)
        {
            _smsConfigurationService = smsConfigurationService;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateSmsTemplateCommand request, CancellationToken cancellationToken)
        {
            return await _smsConfigurationService.UpdateSmsTemplateAsync(request.Id, request.Message);
        }
    }
}

