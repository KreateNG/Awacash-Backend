using System;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transfers.Handlers.Commands.OwnAccountTransfer
{
    public record OwnAccountTransferCommand(
        string DebitAccount, string CreditAccount, decimal Amount, string TransactionPin, string Narration, string TransactionReference, bool AddAsBeneficary) : IRequest<ResponseModel<string>>;
}


