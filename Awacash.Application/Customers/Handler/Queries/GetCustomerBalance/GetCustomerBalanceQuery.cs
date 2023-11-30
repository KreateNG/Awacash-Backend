using Awacash.Application.Customers.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetCustomerBalance
{
    public record GetCustomerBalanceQuery() : IRequest<ResponseModel<CustomerAccountBalanceDTO>>;
}
