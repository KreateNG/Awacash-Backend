using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.DTOs
{
    public record CustomerAccountBalanceDTO(string FullName, decimal Balance);
}
