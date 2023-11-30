using System;
namespace Awacash.Application.Authentication.Common
{
    public record AccountValidationResponse(string AccountId, string LastName, string FirstName, string Hash);
}

