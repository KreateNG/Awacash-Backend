using System;
using Awacash.Domain.FilterModels;

namespace Awacash.Application.Wallets.FilterModels
{
    public class WalletFilterModel : BaseFilterModel
    {
        public string? Status { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
    }
}

