using System;
using Awacash.Domain.Entities;

namespace Awacash.Contracts.Customers
{
    public record UpadateKycDocsRequest(string IDBase64, FileType IDType, string UtilityBase64, string? IDNumber);
}

