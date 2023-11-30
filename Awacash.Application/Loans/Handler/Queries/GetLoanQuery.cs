using System;
using Awacash.Application.Loans.Services;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Queries;

public record GetLoanQuerry() : IRequest<ResponseModel<List<LoanModel>>>;
public class GetLoanQuerryHandler : IRequestHandler<GetLoanQuerry, ResponseModel<List<LoanModel>>>
{
    private readonly ILoanService _loanService;

    public GetLoanQuerryHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }

    public async Task<ResponseModel<List<LoanModel>>> Handle(GetLoanQuerry request, CancellationToken cancellationToken)
    {
        return await _loanService.GetLoanByCustomerId();
    }
}

