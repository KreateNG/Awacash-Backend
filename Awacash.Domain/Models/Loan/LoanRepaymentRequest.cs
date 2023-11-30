namespace Awacash.Domain.Models.Loan;

public class LoanRepaymentRequest
{
    public decimal amount { get; set; }
    public string accountNumber { get; set; }
    public string principalNarration { get; set; }
    public string interestNarration { get; set; }
    public string feeNarration { get; set; }
    public string transactionPin { get; set; }
    public bool isTermination { get; set; }
}

public class LoanRepaymentModel
{
    public string loanAccountNumber { get; set; }
    public string loanAmount { get; set; }
    public string totalRepaymentAmount { get; set; }
    public string principalRepaid { get; set; }
    public string interestRepaid { get; set; }
    public string lastPaymentDate { get; set; }
}

