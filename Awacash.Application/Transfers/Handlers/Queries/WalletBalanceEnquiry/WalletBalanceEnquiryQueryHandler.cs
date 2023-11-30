using Awacash.Application.Transactions.Services;
using Awacash.Application.Transfers.Handlers.Queries.WalletBalanceEnquiry;
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
    public class WalletBalanceEnquiryQueryHandler : IRequestHandler<WalletBalanceEnquiryQuery, ResponseModel<BalanceEnquiryResponse>>
    {
        private readonly ITransferService _transferService;

        public WalletBalanceEnquiryQueryHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<BalanceEnquiryResponse>> Handle(WalletBalanceEnquiryQuery request, CancellationToken cancellationToken)
        {
            return await _transferService.GetWalletBalance(request.AccountNumber);
        }
    }
}
