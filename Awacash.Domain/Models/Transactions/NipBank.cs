using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.Transactions
{
    public record NipBank(
        string BankCode,
        string BankName);
}
