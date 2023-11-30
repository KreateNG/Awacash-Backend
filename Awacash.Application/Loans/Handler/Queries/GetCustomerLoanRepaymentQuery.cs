using System;
using Awacash.Application.Loans.Services;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Loans.Handler.Queries
{
    public record GetCustomerLoanRepaymentQuery(string AccountNumber) : IRequest<ResponseModel<LoanRepaymentModel>>;
    public class GetCustomerLoanRepaymentQueryHandler : IRequestHandler<GetCustomerLoanRepaymentQuery, ResponseModel<LoanRepaymentModel>>
    {
        private readonly ILoanService _loanService;

        public GetCustomerLoanRepaymentQueryHandler(ILoanService loanService)
        {
            _loanService = loanService;
        }
        public async Task<ResponseModel<LoanRepaymentModel>> Handle(GetCustomerLoanRepaymentQuery request, CancellationToken cancellationToken)
        {
            return await _loanService.GetCustomerTotalLoanRepayment(request.AccountNumber);
        }
    }
}

