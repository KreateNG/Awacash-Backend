using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.InitializeCustomerBvnAuth
{
    public class InitializeCustomerBvnAuthQueryHandler : IRequestHandler<InitializeCustomerBvnAuthQuery, ResponseModel<string>>
    {
        private readonly ICustomerService _customerService;

        public InitializeCustomerBvnAuthQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<string>> Handle(InitializeCustomerBvnAuthQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.InitializeBvnAuthenticationAsync(request.Bvn);

        }
    }
}
