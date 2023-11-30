using System;
using Awacash.Application.Loans.Services;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Queries;

public record GetLoanByStatusQuerry(string Status) : IRequest<ResponseModel<List<LoanStatusModel>>>;

public class GetLoanByStatusQuerryHandler : IRequestHandler<GetLoanByStatusQuerry, ResponseModel<List<LoanStatusModel>>>
{
    private readonly ILoanService _loanService;

    public GetLoanByStatusQuerryHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ResponseModel<List<LoanStatusModel>>> Handle(GetLoanByStatusQuerry request, CancellationToken cancellationToken)
    {
        return await _loanService.GetCustomerLoanByStatus(request.Status);
    }
}

