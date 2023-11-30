using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetSingleCustomer
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ResponseModel<CustomerDTO>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerByIdQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<CustomerDTO>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomerByIdAsync(request.id);
        }
    }
}
