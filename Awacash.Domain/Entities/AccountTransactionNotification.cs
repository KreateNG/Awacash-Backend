using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class AccountTransactionNotification: BaseEntity
    {
        public string? Originatoraccountnumber { get; set; }
        public string? Originatorname { get; set; }
        public string? Amount { get; set;}
        public string? Craccountname { get; set; }
        public string? Craccount { get; set; }
        public string? Paymentreference { get; set; }
        public string? Bankname { get; set; }
        public string? Bankcode { get; set; }
        public string? Sessionid { get; set; }
        public string? Narration { get; set; }
        public bool IsVerify { get; set; }
        public string? TransactionReference { get; set; }
    }
}
