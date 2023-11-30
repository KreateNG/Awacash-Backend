using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;

namespace Awacash.Application.Savings.DTOs
{
    public class SavingDTO : BaseDTO
    {
        public string? CustomerId { get; set; }
        public string? Reason { get; set; }
        public int Duration { get; set; }
        public decimal InterestRate { get; set; }
        public DeductionFrequency DeductionFrequency { get; set; }
        public SavingType SavingType { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal DeductionaAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string? SavingAccount { get; set; }
        public string? DeductionAccount { get; set; }
    }
}
