using System;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Application.FeeConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetFeeById
{
    public class GetFeeByIdQueryHandler : IRequestHandler<GetFeeByIdQuery, ResponseModel<FeeConfigurationDto>>
    {
        private readonly IFeeConfigurationService _feeConfigurationService;
        public GetFeeByIdQueryHandler(IFeeConfigurationService feeConfigurationService)
        {
            _feeConfigurationService = feeConfigurationService;
        }
        public async Task<ResponseModel<FeeConfigurationDto>> Handle(GetFeeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _feeConfigurationService.GetFeeConfigurationByIdAsync(request.Id);
        }
    }
}

