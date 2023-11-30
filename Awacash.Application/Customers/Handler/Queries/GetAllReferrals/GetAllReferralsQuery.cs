using System;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Customers.Handler.Queries.GetAllReferrals;

public record GetAllReferralsQuery() : IRequest<ResponseModel<List<ReferralDTO>>>;

public class GetAllReferralsQueryHandler : IRequestHandler<GetAllReferralsQuery, ResponseModel<List<ReferralDTO>>>
{
    private readonly ICustomerService _customerService;

    public GetAllReferralsQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<ResponseModel<List<ReferralDTO>>> Handle(GetAllReferralsQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetReferee();
    }
}

