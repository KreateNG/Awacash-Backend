using System;
using Awacash.Domain.Entities;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Specifications;

namespace Awacash.Application.Wallets.Specifications
{
    public class WalletFilterSpecification : BaseSpecification<Wallet>
    {
        public WalletFilterSpecification(string? firstname, string? lastname, string? phonenumber, string? status)
            : base(
                w =>
                  (string.IsNullOrWhiteSpace(firstname) || w.FirstName.ToLower() == firstname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(lastname) || w.LastName.ToLower() == lastname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(phonenumber) || w.PhoneNumber.ToLower() == phonenumber.ToLower()) &&
                  (string.IsNullOrWhiteSpace(status) || w.Status.ToLower() == status.ToLower())
            )
        {
        }
    }
}

