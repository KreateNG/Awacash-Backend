using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.SavingsConfiguration
{
    public record UpdateSavingConfigurationRequest(string Id, string PlanName, string PlanDescription, int PlanDuration, decimal PlanInterestRate, SavingType SavingType);
}
