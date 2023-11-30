using System;
namespace Awacash.Domain.Models.Loan;

public class LoanModel
{
    public int nominalAccountType { get; set; }
    public string lastDateRestructured { get; set; }
    public int restructuredLoanAmount { get; set; }
    public string restructuredLoanAmountToNaira { get; set; }
    public int restructuredTenure { get; set; }
    public string restructuredCommencementDate { get; set; }
    public int loanPaymentScheduleType { get; set; }
    public bool disablePenalty { get; set; }
    public int securityDeposit { get; set; }
    public int computationMode { get; set; }
    public int computationModeMultiple { get; set; }
    public int linkedCurrentAccountID { get; set; }
    public string product { get; set; }
    public int loanCycle { get; set; }
    public bool useDefaultLaonCycle { get; set; }
    public int loanAmount { get; set; }
    public int discountAmount { get; set; }
    public DateTime interestAccrualCommenceDate { get; set; }
    public string loanFees { get; set; }
    public int moratarium { get; set; }
    public int principalRepaymentType { get; set; }
    public int principalPaymentFrequency { get; set; }
    public int interestRepaymentType { get; set; }
    public int interestAccrualMode { get; set; }
    public int interestPaymentFrequency { get; set; }
    public int interestID { get; set; }
    public int interestRate { get; set; }
    public bool useDefaultInterests { get; set; }
    public int daysAtRisk { get; set; }
    public string realLoanStatus { get; set; }
    public int lendingModel { get; set; }
    public int subLendingModel { get; set; }
    public string otherLendingModel { get; set; }
    public string ippis { get; set; }
    public int economicSector { get; set; }
    public string otherEconomicSector { get; set; }
    public bool moveSecurityDepositOutOfCustomerAccount { get; set; }
    public int defaultingLoanInterest { get; set; }
    public int loanUpdateType { get; set; }
    public int refinanceAmount { get; set; }
    public int refinanceAmountInKobo { get; set; }
    public int newTenure { get; set; }
    public bool restructureFromLoanInception { get; set; }
    public string newCommencementDate { get; set; }
    public string loanRestructure { get; set; }
    public string outstandingBalance { get; set; }
    public bool isSecurityDepositTaken { get; set; }
    public string securityPledged { get; set; }
    public int collateralValue { get; set; }
    public string collateralValueToNaira { get; set; }
    public int restructureLoanCount { get; set; }
    public int refinanceLoanCount { get; set; }
    public int refinancedLoanAmount { get; set; }
    public string refinancedLoanAmountToNaira { get; set; }
    public int refinancedTenure { get; set; }
    public string lastDateRefinanced { get; set; }
    public string refinancedCommencementDate { get; set; }
    public string mdaId { get; set; }
    public string mdaFullName { get; set; }
    public string lendingGroupName { get; set; }
    public string loanWriteOffDate { get; set; }
    public string loanWriteBackDate { get; set; }
    public bool isLoanWrittenOff { get; set; }
    public int loanWriteOffAmount { get; set; }
    public int cummulativeWithdrawalAmount { get; set; }
    public int cummulativeDepositAmount { get; set; }
    public int cummulativeWithdrawalAmountWithExclusions { get; set; }
    public int cummulativeDepositAmountWithExclusions { get; set; }
    public int totalNoOfDebitTrx { get; set; }
    public int totalNoOfCreditTrx { get; set; }
    public string number2 { get; set; }
    public DateTime dateCreated { get; set; }
    public DateTime dateCreatedFinancial { get; set; }
    public string number { get; set; }
    public int productID { get; set; }
    public string productCode { get; set; }
    public string referenceNo { get; set; }
    public string accountOfficer { get; set; }
    public string accountOfficerCode { get; set; }
    public int accountOfficerID { get; set; }
    public StatementPreference statementPreference { get; set; }
    public int notificationPreference { get; set; }
    public string accountOpenningTrackingRef { get; set; }
    public string name { get; set; }
    public int branchID { get; set; }
    public int ledgerBalance { get; set; }
    public int availableBalance { get; set; }
    public bool isRecovaLoan { get; set; }
    public bool isAutoCheck { get; set; }
    public int accessLevel { get; set; }
    public int accountStatus { get; set; }
    public int customerID { get; set; }
    public string introducerName { get; set; }
    public string funderID { get; set; }
    public string funderStr { get; set; }
    public string funderCode { get; set; }
    public string accessRestrictionText { get; set; }
    public int userAccessLevel { get; set; }
    public string withdrawableAmountInNaira { get; set; }
    public string withdrawableAmountWithAccessLevelInNaira { get; set; }
    public string balanceInNaira { get; set; }
    public string ledgerBalanceWithAccessLevelInNaira { get; set; }
    public string availableBalanceInNaira { get; set; }
    public string availableBalanceWithAccessLevelInNaira { get; set; }
    public string nameAndNumber { get; set; }
    public string idAndNumber { get; set; }
    public int id { get; set; }
    public bool isDeleted { get; set; }
    public string mfbCode { get; set; }
    public bool logObject { get; set; }
    public bool useAuditTrail { get; set; }
}

public class StatementPreference
{
    public int delivery { get; set; }
    public int period { get; set; }
}



