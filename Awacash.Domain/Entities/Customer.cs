using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }

        public int PasswordSalt { get; set; }

        public string? SaltedHashedPassword { get; set; }

        public int PasswordTries { get; set; }

        public int? PinSalt { get; set; }

        public string? SaltedHashedPin { get; set; }

        public int PinTries { get; set; }

        public DateTime? PhoneNumberConfirmationDate { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsBvnConfirmed { get; set; }
        public bool IsIdUploaded { get; set; }
        public DateTime? EmailConfirmationDate { get; set; }
        public bool ForceChangeOfPassword { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? DeviceId { get; set; }

        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? NextOfKinName { get; set; }
        public string? NextOfKinRelationship { get; set; }
        public string? NextOfKinPhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? NextOfKinEmail { get; set; }
        public string? NextOfKinAddress { get; set; }
        public string? AccountId { get; set; }
        public string? Bvn { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public string? ReferralCode { get; set; }
        public bool IsForgotPinChangeEnabled { get; set; }
        public virtual Wallet? Wallet { get; set; }
        //public virtual List<Document>? Documents { get; set; } = new List<Document>();
        public virtual List<Referral>? Referrals { get; set; } = new List<Referral>();
        
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {MiddleName}"; ;
            }
        }
    }

}
