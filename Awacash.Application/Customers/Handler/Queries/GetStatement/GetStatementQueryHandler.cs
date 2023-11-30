using System;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Customers.Handler.Queries.GetStatement;

public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, ResponseModel>
{
    private readonly ICustomerService _customerService;
    public GetStatementQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<ResponseModel> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.RequestStatement(request.AccountNumber, request.From.Value, request.To.Value);
    }
}

