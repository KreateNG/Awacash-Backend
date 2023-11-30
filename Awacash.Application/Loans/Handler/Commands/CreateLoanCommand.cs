using System;
using Awacash.Application.Loans.Services;
using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Commands;

public record CreateLoanCommand(int Amount, string Account, string Bvn, LoanDuration Duration, LoanType LoanType, string PlaceOfEmployment, decimal MonthlySalary, EmploymentStatus EmploymentStatus, string Pin) : IRequest<ResponseModel>;
public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, ResponseModel>
{
    private readonly ILoanService _loanService;

    public CreateLoanCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }

    public async Task<ResponseModel> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.CreateLoanRequest(request.Amount, request.Account, request.Bvn, request.Duration, request.LoanType, request.PlaceOfEmployment, request.MonthlySalary, request.EmploymentStatus, request.Pin);
    }
}


