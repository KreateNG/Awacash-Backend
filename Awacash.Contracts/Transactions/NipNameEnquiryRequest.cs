using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Transactions
{
    public record NipNameEnquiryRequest(string BankCode, string AccountNumber);
}
