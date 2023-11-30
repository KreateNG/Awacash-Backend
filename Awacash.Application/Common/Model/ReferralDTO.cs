using System;

namespace Awacash.Application.Common.Model
{
    public class ReferralDTO : BaseDTO
    {
        public string? ReferrerId { get; set; }
        public string? ReferredCustomerId { get; set; }
        public CustomerDTO? ReferredCustomer { get; set; }
    }
}

