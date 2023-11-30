using System;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Users.Handler.Commands.ChangeUserPassword
{
    public record ChangeUserPasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<ResponseModel<string>>;

    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().WithMessage("Confirm password is required");
            RuleFor(x => x.CurrentPassword).NotEmpty().NotNull().WithMessage("Current password is required");
            RuleFor(x => x.NewPassword).NotEmpty().NotNull().WithMessage("New password is required");
            RuleFor(x => x.NewPassword).Equal(x => x.ConfirmPassword).WithMessage("Passwords do not match");
        }
    }
}

