using System;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Shared;

namespace Awacash.Domain.Interfaces
{
    public interface IBankOneAccountService
    {
        Task<ResponseModel<AccountOpeningResponseDto>> AccountOpening(string firstName, string lastName, string middleName, string bvn, string mobile, string gender, string placeOfBirth, string dob, string address, string nin, string email, string referralPhoneNo, string referralName, string nextOfKinPhoneNo, string nextOfKinName);

        Task<ResponseModel<AddAccountResponseDto>> AddAccount(string customerID, string productCode, string email, string accountName, string bvn = default);

        Task<ResponseModel<UpdateAccountDocumentResponseDto>> UpdateAccountDocument(string accountNumber, string customerImage, string customerSignature);

        Task<ResponseModel<UpdateCustomerResponseDto>> UpdateCustomer(string customerId, string bankVerificationNumber);

        Task<ResponseModel<CustomerAccountsResponseDto>> GetAccountsByCustomerId(string customerId);

        Task<ResponseModel<AccountByBvnResponsedDto>> GetAccountsByBvn(string bvn);

        Task<ResponseModel<CustomerAccountsResponseDto>> GetAccountsByAccountNumber(string accountNumber);

        Task<ResponseModel<CreateFixedDepositeResponseDto>> CreateFixDeposit(string customerID, bool isDiscountDeposit, int interestRate, string amount, int tenure, string liquidationAccount, bool applyInterestMonthly, bool applyInterestOnRollOver, bool shouldRollOver);

        Task<ResponseModel<FixedDepositeDetailResponseDto>> GetFixDepositByAccountNumber(string accountNumber);

        Task<ResponseModel<FixedDepositeDetailResponseDto>> GetFixDepositByPhoneNumber(string phone);

        Task<ResponseModel<FixedDepositLiquidation>> FixDepositLiquidation(string accountNumber, int liquidationType, string accountOpenningTrackingRef, string narration);

        Task<ResponseModel<BalanceEnquiryResponse>> BalanceEnquiry(string accountNumber);

        Task<ResponseModel<IntraBankTransferResponse>> IntraBankTransfer(decimal amount, string fromAccount, string destinationAccount, string narration, string channel, string transactionReference);
        Task<ResponseModel<AccountDetailsResponse>> GetAccountDetail(string accountNumber);

        Task<ResponseModel<IntraBankTransferResponse>> Debit(decimal amount, decimal chargeAmount, string fromAccount, string gl, string narration, string channel, string transactionReference);


        Task<ResponseModel<IntraBankTransferResponse>> Credit(decimal amount, decimal chargeAmount, string toAccount, string gl, string narration, string channel, string transactionReference);

        Task<ResponseModel<AccountPNDResponse>> ActivatePND(string accountNumber);
        Task<ResponseModel<StatementResponse>> RequestStatement(string accountNumber, DateTime from, DateTime to);
        Task<ResponseModel<List<TransactionResponseDto>>> GetTransactions(string accountNumber, DateTime from, DateTime to);
    }
}

