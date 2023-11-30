using Awacash.Application.Savings.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Commands.CreateSaving
{
    public record CreateSavingCommand(string? Reason, decimal? TargetAmount, DeductionFrequency? DeductionFrequency, SavingsDuration? SavingsDuration, string? SavingConfigId, string? DeductionAccount) : IRequest<ResponseModel<SavingDTO>>;
}
