using System;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Application.FeeConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetAllFee
{
    public class GetAllFeesQueryHandler : IRequestHandler<GetAllFeesQuery, ResponseModel<List<FeeConfigurationDto>>>
    {
        private readonly IFeeConfigurationService _feeConfigurationService;
        public GetAllFeesQueryHandler(IFeeConfigurationService feeConfigurationService)
        {
            _feeConfigurationService = feeConfigurationService;
        }

        public async Task<ResponseModel<List<FeeConfigurationDto>>> Handle(GetAllFeesQuery request, CancellationToken cancellationToken)
        {
            return await _feeConfigurationService.GetAllFeeConfigurationAsync();
        }
    }
}

