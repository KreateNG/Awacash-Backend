using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Authentication
{
    public record RegisterRequest(
        string? Email,
        string? FirstName,
        string? LastName,
        string? MiddleName,
        string? PhoneNumber,
        string? Password,
        string? ConfirmPassword,
        string? Pin,
        string? ConfirmPin,
        string? Hash,
        string? ReferralCode);

    public class RegisterAccountRequest
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Pin { get; set; }
        public string? ConfirmPin { get; set; }
        public string? AccountId { get; set; }
        public string? Hash { get; set; }
    }
}
