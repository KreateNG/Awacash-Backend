using Awacash.Domain.Common.Models;
using Awacash.Domain.Enums;
using Awacash.Domain.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.FilterModels
{
    public class TransactionFilterModel : BaseFilterModel
    {
        public TransactionType? TransactionType { get; set; }
        public RecordType? RecordType { get; set; }
        public string? AccountNumber { get; set; }
        public string? CustomerId { get; set; }
    }

    public class TransactionRangeModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
