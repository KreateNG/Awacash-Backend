using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Commands.LocalWalletTransfer
{
    public record LocalWalletTransferCommand(
        string DebitAccount, string CreditAccount, decimal Amount, string TransactionPin, string Narration, string TransactionReference, bool AddAsBeneficary) : IRequest<ResponseModel<string>>;
}
