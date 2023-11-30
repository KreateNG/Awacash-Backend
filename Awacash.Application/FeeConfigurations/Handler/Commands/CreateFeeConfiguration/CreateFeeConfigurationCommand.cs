using System;
using Awacash.Domain.Enums;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.FeeConfigurations.Handler.Commands.CreateFeeConfiguration
{
    public record CreateFeeConfigurationCommand(TransactionType? TransactionType, decimal? UpperBound, decimal? LowerBound, decimal? Fee) : IRequest<ResponseModel<bool>>;

    public class CreateFeeConfigurationValidator : AbstractValidator<CreateFeeConfigurationCommand>
    {
        public CreateFeeConfigurationValidator()
        {
            RuleFor(x => x.TransactionType).NotEmpty().NotNull().WithMessage("Transaction type is required");
            RuleFor(x => x.UpperBound).NotEmpty().NotNull().WithMessage("Upper bound is required");
            RuleFor(x => x.LowerBound).NotEmpty().NotNull().WithMessage("Lower bound is required");
            RuleFor(x => x.Fee).NotEmpty().NotNull().WithMessage("Fee is required");
        }
    }
}

