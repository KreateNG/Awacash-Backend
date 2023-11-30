using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Saving : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? SavingAccount { get; set; }
        public string? DeductionAccount { get; set; }
        public string? Reason { get; set; }
        public int Duration { get; set; }
        public decimal InterestRate { get; set; }
        public DeductionFrequency DeductionFrequency { get; set; }
        public SavingType SavingType { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal DeductionaAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
