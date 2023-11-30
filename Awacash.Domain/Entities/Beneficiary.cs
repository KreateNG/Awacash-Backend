using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Beneficiary: BaseEntity
    {
        public string? PreferedName { get; set; }
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public string? BankCode { get; set; }
        public string? BankName { get; set; }

        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MeterNumber { get; set; }
        public string? Biller { get; set; }
        public BeneficaryType? BeneficaryType { get; set; }
        public Status Status { get; set; }
    }
}
