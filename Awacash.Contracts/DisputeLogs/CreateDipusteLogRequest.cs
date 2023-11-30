using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.DisputeLogs
{
    public record CreateDipusteLogRequest(string AccountNumber, string Email, string PhoneNumber, decimal Amount, DateTime TransactionDate, string Comment);
}
