using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Commands.CreateSaving;

public class CreateSavingCommandValidator : AbstractValidator<CreateSavingCommand>
{
    public CreateSavingCommandValidator()
    {
        RuleFor(x => x.DeductionFrequency).NotEmpty().WithMessage("Deduction frequency is required");
        RuleFor(x => x.TargetAmount).NotEmpty().WithMessage("Target amount is required");
        RuleFor(x => x.SavingConfigId).NotEmpty().WithMessage("Saving configuration Id is required");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason for saving is required");
        RuleFor(x => x.DeductionAccount).NotEmpty().WithMessage("Deduction account for is required");
        RuleFor(x => x.SavingsDuration).NotEmpty().WithMessage("Duration for is required");
    }
}
