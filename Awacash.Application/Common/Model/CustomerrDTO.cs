using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Common.Model
{
    public class CustomerDTO : BaseDTO
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsBvnConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool ForceChangeOfPassword { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? FullName { get; set; }
        public string? DeviceId { get; set; }

        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? NextOfKinName { get; set; }
        public string? NextOfKinRelationship { get; set; }
        public string? NextOfKinPhoneNumber { get; set; }
        public string? Bvn { get; set; }
        public string? ReferralCode { get; set; }
        public bool IsIdUploaded { get; set; }

    }
}
