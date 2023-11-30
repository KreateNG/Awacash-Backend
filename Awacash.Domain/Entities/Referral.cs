using System;
namespace Awacash.Domain.Entities
{
    public class Referral : BaseEntity
    {
        public string ReferrerId { get; set; } = default!;
        public string ReferredCustomerId { get; set; } = default!;
        //public virtual Customer Referrer { get; set; } = default!;
        public virtual Customer ReferredCustomer { get; set; } = default!;
    }
}

