using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Transactions
{
    public record AccountTransactionNotificationRequest(string Originatoraccountnumber, string Originatorname, string Amount, string Craccountname, string Craccount, string Paymentreference, string Bankname, string Bankcode, string Sessionid, string Narration);
}
