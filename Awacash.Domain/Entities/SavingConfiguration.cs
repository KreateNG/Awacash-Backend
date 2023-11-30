using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class SavingConfiguration : BaseEntity
    {
        public string? PlanName { get; set; }
        public string? PlanDescription { get; set; }
        public int PlanDuration { get; set; }
        public decimal PlanInterestRate { get; set; }
        public SavingType SavingType { get; set; }
        public bool Status { get; set; }
        public string? ProductCode { get; set; }
    }
}
