using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Queries.GetAllSavingConfiguration
{
    public class GetAllSavingConfigurationQueryHandler : IRequestHandler<GetAllSavingConfigurationQuery, ResponseModel<List<SavingConfigurationDTO>>>
    {
        private readonly ISavingConfigurationService _savingConfigurationService;

        public GetAllSavingConfigurationQueryHandler(ISavingConfigurationService savingConfigurationService)
        {
            _savingConfigurationService = savingConfigurationService;
        }

        public async Task<ResponseModel<List<SavingConfigurationDTO>>> Handle(GetAllSavingConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await _savingConfigurationService.GetAllSavingConfigurationAsync();
        }
    }
}
