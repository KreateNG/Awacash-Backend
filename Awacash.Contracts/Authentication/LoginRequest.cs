using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Authentication
{
    public record LoginRequest(
        string Email,
        string Password,
        string? DeviceId);

    public record AdminLoginRequest(
        string Email,
        string Password);
}
