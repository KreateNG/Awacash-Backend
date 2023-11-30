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

namespace Awacash.Application.Transfers.Handlers.Commands.InterBankWalletTransfer
{
    public class InterBankWalletTransferCommandHandler : IRequestHandler<InterBankWalletTransferCommand, ResponseModel<NipTransferResponse>>
    {
        private readonly ITransferService _transferService;

        public InterBankWalletTransferCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }
        public async Task<ResponseModel<NipTransferResponse>> Handle(InterBankWalletTransferCommand request, CancellationToken cancellationToken)
        {
            return await _transferService.PostWalletInterBankTransfer(request.DebitAccount, request.CreditAccount, request.bankCode, request.Amount, request.Narration, request.TransactionPin, request.TransactionReference, request.AddAsBeneficary);
        }
    }
}
