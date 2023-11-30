using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class DisputeLog: BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? AccountNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Comment { get; set; }
        public bool IsSettled { get; set; }
        public Customer? Customer { get; set; }
    }
}
