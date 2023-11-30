using System;
namespace Awacash.Domain.Models.Loan;

public class LoanStatusModel
{
    public string id { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime modifiedDate { get; set; }
    public string amount { get; set; }
    public string customerId { get; set; }
    public int tenor { get; set; }
    public string tenorType { get; set; }
    public string accountNumber { get; set; }
    public string bvn { get; set; }
    public string approvedBy { get; set; }
    public string confirmedComment { get; set; }
    public string approvalComment { get; set; }
    public string confirmedBy { get; set; }
    public DateTime confirmedDate { get; set; }
    public string loanRequestStatus { get; set; }
    public DateTime approvedDate { get; set; }
    public string loanType { get; set; }
    public string bankOneResponse { get; set; }
    public string lienAmount { get; set; }
    public string lastName { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public DateTime dateOfBirth { get; set; }
    public string gender { get; set; }
    public string lienReference { get; set; }
}

