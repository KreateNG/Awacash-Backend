using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Enums
{
    public enum TransactionType
    {
        LocalTransfer = 1,
        InterBankTransfer,
        AirTime,
        BillPayment,
        CardRequest,
        Saving
    }
}
