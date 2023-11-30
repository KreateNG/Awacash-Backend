using System;
using Awacash.Application.Common.Model;
using Awacash.Domain.Entities;

namespace Awacash.Application.Wallets.DTOs
{
    public class WalletDTO : BaseDTO
    {
        public string? CustomerId { get; set; }
        public decimal? Balance { get; set; }
        public string? Status { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
    }
}

