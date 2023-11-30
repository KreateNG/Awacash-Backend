using Awacash.Application.FeeConfigurations.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetFee
{
    public class GetFeeQueryHandler : IRequestHandler<GetFeeQuery, ResponseModel<decimal>>
    {
        private readonly IFeeConfigurationService _feeConfigurationService;

        public GetFeeQueryHandler(IFeeConfigurationService feeConfigurationService)
        {
            _feeConfigurationService = feeConfigurationService;
        }

        public async Task<ResponseModel<decimal>> Handle(GetFeeQuery request, CancellationToken cancellationToken)
        {
            return await _feeConfigurationService.GetTransactionFeeAsync(request.Amount, request.TransactionType);
        }
    }
}
