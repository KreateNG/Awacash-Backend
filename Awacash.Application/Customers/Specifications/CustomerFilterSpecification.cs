using Awacash.Domain.Entities;
using Awacash.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Awacash.Application.Customers.Specifications
{
    public class CustomerFilterSpecification : BaseSpecification<Customer>
    {
        public CustomerFilterSpecification(string firstname, string lastname, string middlename, string email, string phonenumber) 
            : base(
                f => 
                  (string.IsNullOrWhiteSpace(firstname) || f.FirstName.ToLower() == firstname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(lastname) || f.LastName.ToLower() == lastname.ToLower()) &&
                  (string.IsNullOrWhiteSpace(middlename) || f.MiddleName.ToLower() == middlename.ToLower()) &&
                  (string.IsNullOrWhiteSpace(email) || f.Email.ToLower() == email.ToLower()) &&
                  (string.IsNullOrWhiteSpace(phonenumber) || f.PhoneNumber.ToLower() == phonenumber.ToLower())
            )
        {
        }
    }
}
