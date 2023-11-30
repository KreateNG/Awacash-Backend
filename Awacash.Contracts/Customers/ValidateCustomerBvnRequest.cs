using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Customers
{
    public record ValidateCustomerBvnRequest(string FirstName, string LastName, DateTime DateOfBirth, string AccessToken);
}
