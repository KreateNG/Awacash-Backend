using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;

namespace Awacash.Application.Authentication.Common
{
    public record AuthenticationResult(CustomerDTO? User, string? Token, string? RefreshToken, bool IsNewDevice = false);
}
