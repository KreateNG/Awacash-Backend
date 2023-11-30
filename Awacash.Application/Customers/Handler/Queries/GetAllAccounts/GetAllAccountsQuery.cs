using System;
using Awacash.Application.Customers.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Customers.Handler.Queries.GetAllAccounts
{
    public class GetAllAccountsQuery : IRequest<ResponseModel<List<AccountDTO>>>
    {

    }
}

