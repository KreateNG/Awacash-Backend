using Awacash.Shared.Models.Paging;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awacash.Application.Customers.FilterModels;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Common.Model;

namespace Awacash.Application.Customers.Handler.Queries.GetPaginatedCustomers
{
    public class GetPaginatedCustomerQuery : CustomerFilterModel, IRequest<ResponseModel<PagedResult<CustomerDTO>>>
    {
        
    }
}
