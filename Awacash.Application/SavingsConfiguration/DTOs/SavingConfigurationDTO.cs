using Awacash.Domain.Enums;
using Awacash.Domain.Models.DTO;

namespace Awacash.Application.SavingsConfiguration.DTOs
{
    public record SavingConfigurationDTO(string? PlanName, string? PlanDescription, int PlanDuration, decimal PlanInterestRate, SavingType SavingType, bool Status, string? ProductCode) : BaseDTO;

}
