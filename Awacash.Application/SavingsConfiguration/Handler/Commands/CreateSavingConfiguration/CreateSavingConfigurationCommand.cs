using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Handler.Commands.CreateSavingConfiguration
{
    public record CreateSavingConfigurationCommand(string? PlanName, string? PlanDescription, int? PlanDuration, decimal? PlanInterestRate, SavingType? SavingType, string ProductCode) : IRequest<ResponseModel<SavingConfigurationDTO>>;
}
