using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Handler.Queries.GetAllCustomers;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.GetCustomerBalance
{
    public class GetCustomerBalanceQueryHandler: IRequestHandler<GetCustomerBalanceQuery, ResponseModel<CustomerAccountBalanceDTO>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerBalanceQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<CustomerAccountBalanceDTO>> Handle(GetCustomerBalanceQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomerBalanceAsync();
        }
    }
}
