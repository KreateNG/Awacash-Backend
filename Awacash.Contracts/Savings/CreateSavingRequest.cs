using Awacash.Domain.Enums;

namespace Awacash.Contracts.Savings
{
    public record CreateSavingRequest(string? Reason, decimal? TargetAmount, DeductionFrequency? DeductionFrequency, SavingsDuration? SavingsDuration, string? SavingConfigId, string? DeductionAccount);
}
