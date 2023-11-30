using Awacash.Application.Common.Interfaces.Authentication;
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

namespace Awacash.Application.Customers.Handler.Queries.GetCustomerProfile
{
    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, ResponseModel<CustomerDTO>>
    {
        private readonly ICustomerService _customerService;
        private readonly ICurrentUser _currentUser;

        public GetCustomerProfileQueryHandler(ICustomerService customerService, ICurrentUser currentUser)
        {
            _customerService = customerService;
            _currentUser = currentUser;
        }

        public Task<ResponseModel<CustomerDTO>> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            return _customerService.GetCustomerByIdAsync(_currentUser.GetCustomerId());
        }
    }
}
