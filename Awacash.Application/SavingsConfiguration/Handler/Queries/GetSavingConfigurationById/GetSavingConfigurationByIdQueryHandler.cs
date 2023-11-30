using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Queries.GetSavingConfigurationById
{
    public class GetSavingConfigurationByIdQueryHandler : IRequestHandler<GetSavingConfigurationByIdQuery, ResponseModel<SavingConfigurationDTO>>
    {
        private readonly ISavingConfigurationService _savingConfigurationService;

        public GetSavingConfigurationByIdQueryHandler(ISavingConfigurationService savingConfigurationService)
        {
            _savingConfigurationService = savingConfigurationService;
        }

        public async Task<ResponseModel<SavingConfigurationDTO>> Handle(GetSavingConfigurationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _savingConfigurationService.GetSavingConfigurationByIdAsync(request.Id);
        }
    }
}
