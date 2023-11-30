using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Users
{
    public record UpdateUserRequest(string FirstName, string LastName, string Email, string PhoneNumber, string UserName, string RoleId);
}
