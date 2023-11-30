using System;
using Awacash.Application.Transfers.Handlers.Commands.InterBankTransfer;
using Awacash.Application.Transfers.Handlers.Commands.InterBankWalletTransfer;
using Awacash.Application.Transfers.Handlers.Commands.LocalWalletTransfer;
using Awacash.Application.Transfers.Services;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transfers.Handlers.Commands.OwnAccountTransfer
{
    public class OwnAccountTransferCommandHandler : IRequestHandler<OwnAccountTransferCommand, ResponseModel<string>>
    {
        private readonly ITransferService _transferService;

        public OwnAccountTransferCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<string>> Handle(OwnAccountTransferCommand request, CancellationToken cancellationToken)
        {
            return await _transferService.PostOwnAccountTransfer(request.DebitAccount, request.CreditAccount, request.Amount, request.Narration, request.TransactionPin, request.TransactionReference, request.AddAsBeneficary);
        }
    }
}

