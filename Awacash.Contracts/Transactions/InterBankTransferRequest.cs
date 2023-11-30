using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Transactions
{
    public record InterBankTransferRequest(string DebitAccount, string BankCode, string CreditAccount, decimal Amount, string TransactionPin, string Narration, string TransactionReference, bool AddAsBeneficary);
}
