using System;
namespace Awacash.Domain.Models.Loan;

public class LoanBalanceModel
{
    public string loanAccountNo { get; set; }
    public decimal accountBalance { get; set; }
    public decimal principalDueButUnpaid { get; set; }
    public decimal interestDueButUnpaid { get; set; }
    public decimal loanFeeDueButUnPaid { get; set; }
    public decimal penaltyDueButUnpaid { get; set; }
    public int principalPaidTillDate { get; set; }
    public int interestPaidTillDate { get; set; }
    public int totalOutstandingAmount { get; set; }
    public int interestNoYetDue { get; set; }
    public int loanFeeNotYetDue { get; set; }
    public int loanFeePaidTillDate { get; set; }
    public int totalAmountPaidTillDate { get; set; }
    public int loanPenaltyPaidTillDate { get; set; }
}



