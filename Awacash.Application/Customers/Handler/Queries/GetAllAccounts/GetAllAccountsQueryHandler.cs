using System;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Customers.Handler.Queries.GetAllAccounts
{
    public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, ResponseModel<List<AccountDTO>>>
    {
        private readonly ICustomerService _customerService;
        public GetAllAccountsQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<ResponseModel<List<AccountDTO>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetAllCustomerAccountsAsync();
        }
    }
}

