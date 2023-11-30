using Awacash.Application.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.DisputeLogs.DTOs
{
    public class DisputeLogDTO: BaseDTO
    {
        public string? AccountNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Comment { get; set; }
        public bool IsSettled { get; set; }
    }
}
