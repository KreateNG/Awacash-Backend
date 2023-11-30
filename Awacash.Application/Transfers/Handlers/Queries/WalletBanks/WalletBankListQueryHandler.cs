
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

namespace Awacash.Application.Transfers.Handlers.Queries.WalletBanks
{
    internal class WalletBankListQueryHandler : IRequestHandler<WalletBankListQuery, ResponseModel<List<NipBank>>>
    {
        private readonly ITransferService _transferService;

        public WalletBankListQueryHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<List<NipBank>>> Handle(WalletBankListQuery request, CancellationToken cancellationToken)
        {
            return await _transferService.GetWalletNIPBanks();
        }
    }
}
