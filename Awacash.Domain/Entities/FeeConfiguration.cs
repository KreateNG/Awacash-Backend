using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class FeeConfiguration:BaseEntity
    {
        public TransactionType TransactionType { get; set; }

        public decimal UpperBound { get; set; }

        public decimal LowerBound { get; set; }

        public decimal Fee { get; set; }
    }
}
