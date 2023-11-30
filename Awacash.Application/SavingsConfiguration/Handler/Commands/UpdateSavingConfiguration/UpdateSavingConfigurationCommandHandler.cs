using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Commands.UpdateSavingConfiguration
{
    public class UpdateSavingConfigurationCommandHandler : IRequestHandler<UpdateSavingConfigurationCommand, ResponseModel<SavingConfigurationDTO>>
    {
        private readonly ISavingConfigurationService _savingConfigurationService;

        public UpdateSavingConfigurationCommandHandler(ISavingConfigurationService savingConfigurationService)
        {
            _savingConfigurationService = savingConfigurationService;
        }

        public async Task<ResponseModel<SavingConfigurationDTO>> Handle(UpdateSavingConfigurationCommand request, CancellationToken cancellationToken)
        {
            return await _savingConfigurationService.UpdateSavingConfigurationAsync(request.Id, request.PlanName, request.PlanDescription, request.PlanDuration, request.PlanInterestRate, request.SavingType);
        }
    }
}
