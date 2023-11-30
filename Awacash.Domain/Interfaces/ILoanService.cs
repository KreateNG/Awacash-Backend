using System;
using Awacash.Domain.Models.Loan;
using Awacash.Shared;

namespace Awacash.Domain.Interfaces;

public interface ILoanProviderService
{
    Task<ResponseModel<BaseLoanResponseData>> CreateLoan(CreateLoanRequest createLoanRequest);
    Task<ResponseModel<List<LoanStatusModel>>> GetLoansByStatus(string status);
    Task<ResponseModel<List<LoanModel>>> GetLoanByCustomerId(string customerId);
    Task<ResponseModel<List<LoanBalanceModel>>> GetLoanBalance(string customerId);
    Task<ResponseModel<LoanRepaymentModel>> GetLoanTotalRepayment(string accountNumber);
    Task<ResponseModel<LoanRepaymentModel>> GetLoanLastRepaymentDetails(string accountNumber);
    Task<ResponseModel<BaseLoanResponseData>> RepayLoan(LoanRepaymentRequest loanRepaymentRequest);
}

