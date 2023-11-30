using Awacash.Application.Transactions.Services;
using Awacash.Application.Transfers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Commands.LocalTransfer
{
    class LocalTransferCommandHandler : IRequestHandler<LocalTransferCommand, ResponseModel<string>>
    {
        private readonly ITransferService _transferService;

        public LocalTransferCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<string>> Handle(LocalTransferCommand request, CancellationToken cancellationToken)
        {
           return await _transferService.PostLocalTransfer(request.DebitAccount, request.CreditAccount, request.Amount, request.Narration, request.TransactionPin, request.TransactionReference, request.AddAsBeneficary);
        }
    }
}
