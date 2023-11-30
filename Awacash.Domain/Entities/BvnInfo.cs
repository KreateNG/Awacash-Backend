using System;
namespace Awacash.Domain.Entities
{
    public class BvnInfo : BaseEntity
    {
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public string? Surname { get; set; }
        public string? MiddleName { get; set; }
        public string? FirstName { get; set; }
        public string? Nationality { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LgaOfOrigin { get; set; }
        public string? StateOfResidence { get; set; }
        public string? LgaOfResidence { get; set; }
        public string? Email { get; set; }
        public string? StateOfCapture { get; set; }
        public string? LgaOfCapture { get; set; }
        public string? Nin { get; set; }
        public string? PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? EnrollmentDate { get; set; }
        public string? EnrollBankCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? NameOnCard { get; set; }
        public string? Title { get; set; }
        public string? BranchName { get; set; }
        public int AccountDetailId { get; set; }
        public int ImageDetailsId { get; set; }
    }
}

