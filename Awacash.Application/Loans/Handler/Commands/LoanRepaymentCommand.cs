using System;
using Awacash.Application.Loans.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Commands;

public record LoanRepaymentCommand(decimal Amount, string AccountNumber, string Pin, bool IsTermination) : IRequest<ResponseModel>;

public class LoanRepaymentCommandHandler : IRequestHandler<LoanRepaymentCommand, ResponseModel>
{
    private readonly ILoanService _loanService;

    public LoanRepaymentCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }

    public async Task<ResponseModel> Handle(LoanRepaymentCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.RepayLoanRequest(request.Amount, request.AccountNumber, request.Pin, request.IsTermination);
    }
}


