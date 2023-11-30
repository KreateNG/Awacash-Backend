using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Authentication
{
    public record VerifyPhoneNumberRequest(string PhoneNumber, string Code, string Hash);
    public record VerifyAccountNumberRequest(string Code, string Hash);
}
