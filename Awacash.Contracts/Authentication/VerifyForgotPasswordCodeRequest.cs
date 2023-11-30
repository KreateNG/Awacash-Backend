using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Authentication
{
    public record VerifyForgotPasswordCodeRequest(string Code, string Hash);
}
