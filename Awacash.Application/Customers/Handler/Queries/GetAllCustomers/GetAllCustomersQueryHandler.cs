using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, ResponseModel<List<CustomerDTO>>>
    {
        private readonly ICustomerService _customerService;

        public GetAllCustomersQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<List<CustomerDTO>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetAllCustomersAsync();
        }
    }
}
