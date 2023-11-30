using Awacash.Application.Transactions.Services;
using Awacash.Application.Transfers.Services;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Queries.BalanceEnquiry
{
    public class BalanceEnquiryQueryHandler : IRequestHandler<BalanceEnquiryQuery, ResponseModel<BalanceEnquiryResponse>>
    {
        private readonly ITransferService _transferService;

        public BalanceEnquiryQueryHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<BalanceEnquiryResponse>> Handle(BalanceEnquiryQuery request, CancellationToken cancellationToken)
        {
            return await _transferService.GetBalance(request.AccountNumber);
        }
    }
}
