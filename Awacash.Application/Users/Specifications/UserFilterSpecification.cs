using Awacash.Domain.IdentityModel;
using Awacash.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Specifications
{
    public class UserFilterSpecification : BaseSpecification<ApplicationUser>
    {
        public UserFilterSpecification(string id, string firstname, string lastname, string email, string phonenumber, bool? isActive)
            : base(
                f =>
                  (string.IsNullOrWhiteSpace(id) || f.Id == id) &&
                  (string.IsNullOrWhiteSpace(firstname) || f.FirstName.ToLower() == firstname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(lastname) || f.LastName.ToLower() == lastname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(email) || f.Email.ToLower() == email.ToLower()) &&
                  (string.IsNullOrWhiteSpace(phonenumber) || f.PhoneNumber.ToLower() == phonenumber.ToLower()) &&
                  (isActive.HasValue || f.IsActive == isActive.Value)
            )
        {
        }
    }
}
