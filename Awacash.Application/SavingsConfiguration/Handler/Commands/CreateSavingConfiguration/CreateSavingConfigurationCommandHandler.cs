using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Commands.CreateSavingConfiguration
{
    public class CreateSavingConfigurationCommandHandler : IRequestHandler<CreateSavingConfigurationCommand, ResponseModel<SavingConfigurationDTO>>
    {
        private readonly ISavingConfigurationService _savingConfigurationService;

        public CreateSavingConfigurationCommandHandler(ISavingConfigurationService savingConfigurationService)
        {
            _savingConfigurationService = savingConfigurationService;
        }

        public async Task<ResponseModel<SavingConfigurationDTO>> Handle(CreateSavingConfigurationCommand request, CancellationToken cancellationToken)
        {
            return await _savingConfigurationService.CreateSavingConfigurationAsync(request.PlanName, request.PlanDescription, request.PlanDuration.Value, request.PlanInterestRate.Value, request.SavingType.Value, request.ProductCode);
        }
    }
}
