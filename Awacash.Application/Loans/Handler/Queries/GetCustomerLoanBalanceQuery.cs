using System;
using Awacash.Application.Loans.Services;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Queries;

public record GetCustomerLoanBalanceQuery() : IRequest<ResponseModel<List<LoanBalanceModel>>>;

public class GetCustomerLoanBalanceQueryHandler : IRequestHandler<GetCustomerLoanBalanceQuery, ResponseModel<List<LoanBalanceModel>>>
{
    private readonly ILoanService _loanService;

    public GetCustomerLoanBalanceQueryHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public Task<ResponseModel<List<LoanBalanceModel>>> Handle(GetCustomerLoanBalanceQuery request, CancellationToken cancellationToken)
    {
        return _loanService.GetCustomerLoanBalance();
    }
}
