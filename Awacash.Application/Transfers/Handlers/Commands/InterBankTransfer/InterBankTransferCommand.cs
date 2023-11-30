using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Commands.InterBankTransfer
{
    public record InterBankTransferCommand(string DebitAccount, string bankCode, string CreditAccount, decimal Amount, string TransactionPin, string Narration, string TransactionReference, bool AddAsBeneficary) : IRequest<ResponseModel<NipTransferResponse>>;
}
