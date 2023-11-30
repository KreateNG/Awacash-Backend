using System;
namespace Awacash.Contracts.Customers
{
    public record UpdateCustomerNextOfKinRequest(string? Address, string? State, string? City, string? NextOfKinName, string? NextOfKinRelationship, string? NextOfKinPhoneNumber, string? Country, string? NextOfKinEmail, string? NextOfKinAddress);
}

