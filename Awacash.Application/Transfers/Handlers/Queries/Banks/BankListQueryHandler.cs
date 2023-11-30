
using Awacash.Application.Transfers.Services;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Queries.Banks
{
    internal class BankListQueryHandler : IRequestHandler<BankListQuery, ResponseModel<List<NipBank>>>
    {
        private readonly ITransferService _transferService;

        public BankListQueryHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<List<NipBank>>> Handle(BankListQuery request, CancellationToken cancellationToken)
        {
            return await _transferService.GetNIPBanks();
        }
    }
}
