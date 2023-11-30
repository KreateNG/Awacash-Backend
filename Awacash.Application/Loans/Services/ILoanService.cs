using System;
using System.Security.Principal;
using Awacash.Domain.Enums;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;

namespace Awacash.Application.Loans.Services
{
    public interface ILoanService
    {
        Task<ResponseModel> CreateLoanRequest(int amount, string account, string bvn, LoanDuration duration, LoanType loanType, string placeOfEmployment, decimal monthlySalary, EmploymentStatus employmentStatus, string transactionPin);
        Task<ResponseModel> RepayLoanRequest(decimal amount, string accountNumber, string transactionPin, bool isTermination);
        Task<ResponseModel<LoanRepaymentModel>> GetCustomerTotalLoanRepayment(string accountNumber);
        Task<ResponseModel<List<LoanBalanceModel>>> GetCustomerLoanBalance();
        Task<ResponseModel<List<LoanModel>>> GetLoanByCustomerId();
        Task<ResponseModel<List<LoanStatusModel>>> GetCustomerLoanByStatus(string status);

    }
}

