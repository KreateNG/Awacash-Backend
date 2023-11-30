using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetPaginatedCustomers
{
    public class GetPaginatedCustomerQueryHandler : IRequestHandler<GetPaginatedCustomerQuery, ResponseModel<PagedResult<CustomerDTO>>>
    {
        private readonly ICustomerService _customerService;

        public GetPaginatedCustomerQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<PagedResult<CustomerDTO>>> Handle(GetPaginatedCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetPaginatedCustomerAsync(request);
        }
    }
}
