using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetSingleCustomer
{
    public record GetCustomerByIdQuery(string id):IRequest<ResponseModel<CustomerDTO>>;
}
