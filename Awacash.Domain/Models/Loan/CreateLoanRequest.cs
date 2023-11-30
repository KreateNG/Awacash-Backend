using System;
namespace Awacash.Domain.Models.Loan;

public class CreateLoanRequest
{
    public int Amount { get; set; }
    public string CustomerId { get; set; }
    public int Tenor { get; set; }
    public string AccountNumber { get; set; }
    public string TransactionPin { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Bvn { get; set; }
    public string LoanType { get; set; }
    public string TenorType { get; set; }
    public string Nationality { get; set; }
    public string MariratalStatus { get; set; }
    public string MobileNumber { get; set; }
    public string PrimaryAddress { get; set; }
    public string PrimaryCityLGA { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string EmploymentStatus { get; set; }
}
public class BaseLoanResponseData
{
    public string value { get; set; }
    public string error { get; set; }
    public bool isSuccessful { get; set; }
    public string message { get; set; }
    public string responseCode { get; set; }
}


