using System;
using Awacash.Application.FeeConfigurations.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Commands.CreateFeeConfiguration
{
    public class CreateFeeConfigurationCommandHandler : IRequestHandler<CreateFeeConfigurationCommand, ResponseModel<bool>>
    {
        private readonly IFeeConfigurationService _feeConfigurationService;
        public CreateFeeConfigurationCommandHandler(IFeeConfigurationService feeConfigurationService)
        {
            _feeConfigurationService = feeConfigurationService;
        }

        public async Task<ResponseModel<bool>> Handle(CreateFeeConfigurationCommand request, CancellationToken cancellationToken)
        {
            return await _feeConfigurationService.CreateFeeConfigurationAsync(request.TransactionType, request.UpperBound, request.LowerBound, request.Fee);
        }
    }
}

