using System;
namespace Awacash.Domain.Models.BankOneAccount
{
    public class AccountOpeningResponseDto
    {
        public string? AccountNumber { get; set; }
        public string? BankoneAccountNumber { get; set; }
        public string? CustomerID { get; set; }
        public string? FullName { get; set; }
        public string? CreationMessage { get; set; }
        public int Id { get; set; }
    }


    public class AddAccountResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string? CustomerIDInString { get; set; }
        public string? Message { get; set; }
        public string? TransactionTrackingRef { get; set; }
        public string? AccountNumber { get; set; }
    }



    public class UpdateAccountDocumentResponseDto
    {
        public string? AccountNumber { get; set; }
        public string? CustomerImage { get; set; }
        public string? CustomerSignature { get; set; }
    }

    public class UpdateCustomerResponseDto
    {
        public string? CustomerId { get; set; }
        public string? BankVerificationNumber { get; set; }
    }

    public class AccountDetail
    {
        public string? AccessLevel { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountType { get; set; }
        public string? AvailableBalance { get; set; }
        public string? Branch { get; set; }
        public string? CustomerID { get; set; }
        public string? AccountName { get; set; }
        public string? ProductCode { get; set; }
        public string? DateCreated { get; set; }
        public string? LastActivityDate { get; set; }
        public string? LedgerBalance { get; set; }
        public string? Nuban { get; set; }
        public string? ReferenceNo { get; set; }
        public string? WithdrawableAmount { get; set; }
        public string? KycLevel { get; set; }
    }

    public class CustomerAccountsResponseDto
    {
        public string? Address { get; set; }
        public string? Age { get; set; }
        public string? Bvn { get; set; }
        public string? BranchCode { get; set; }
        public string? CustomerID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? LocalGovernmentArea { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public List<AccountDetail>? Accounts { get; set; }
        public string? PostalAddress { get; set; }
        public string? BusinessPhoneNo { get; set; }
        public string? TaxIDNo { get; set; }
        public string? BusinessName { get; set; }
    }

    public class AccountByBvnResponsedDto
    {
        public string? AccountNumber { get; set; }
        public string? BankVerificationNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CustomerID { get; set; }
        public string? ProductCode { get; set; }
        public string? LastName { get; set; }
        public string? OtherNames { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? AccountStatus { get; set; }
        public string? DateCreated { get; set; }
        public string? AccountType { get; set; }
    }

    public class CreateFixedDepositeResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string? CustomerIDInString { get; set; }
        public string? Message { get; set; }
        public string? TransactionTrackingRef { get; set; }
    }


    public class FixedDepositeDetailResponseDto
    {
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? CustomerID { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? LiquidationAccount { get; set; }
        public bool ShouldRollOver { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string? InterestAccrualCommencementDate { get; set; }
        public int InterestRate { get; set; }
        public int TenureInDays { get; set; }
        public bool ApplyMonthlyInterest { get; set; }
        public bool ApplyInterestOnRollOver { get; set; }
        public string? MaturationDate { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountOpenningTrackingRef { get; set; }
    }

    public class FixedDepositLiquidation
    {
        public bool IsSuccessful { get; set; }
        public string? CustomerIDInString { get; set; }
        public string? Message { get; set; }
        public string? TransactionTrackingRef { get; set; }
    }

    public class BalanceEnquiryResponse
    {
        public string? AccountNumber { get; set; }
        public string? Name { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal WithdrawableAmount { get; set; }
        public string? RequestId { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsBankOrSuspenseAssetAccount { get; set; }
        public string? Code { get; set; }
    }


    public class IntraBankTransferResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ResponseMessage { get; set; }
        public string? ResponseCode { get; set; }
        public string? Reference { get; set; }
        public string? RetrievalReference { get; set; }
    }

    public class AccountDetailsResponse
    {
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? Nuban { get; set; }
        public string? Number { get; set; }
        public string? ProductCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bvn { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LedgerBalance { get; set; }
        public string? Status { get; set; }
        public string? Tier { get; set; }
        public int StatusCode { get; set; }
        public decimal? MaximumBalance { get; set; }
        public decimal? MaximumDeposit { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ResponseMessage { get; set; }
        public string? PndStatus { get; set; }
        public string? LienStatus { get; set; }
        public string FreezeStatus { get; set; }
        public bool RequestStatus { get; set; }
        public string? ResponseDescription { get; set; }
        public string? ResponseStatus { get; set; }
    }


    public class AccountPNDResponse
    {
        public bool RequestStatus { get; set; }
        public string? ResponseDescription { get; set; }
        public string? ResponseStatus { get; set; }

    }
    public class StatementResponse
    {
        public bool IsSuccessful { get; set; }
        public string? CustomerIDInString { get; set; }
        public string? Message { get; set; }
        public string? TransactionTrackingRef { get; set; }
        public string? AccountNumber { get; set; }
    }

}

