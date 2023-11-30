using System;
using FluentValidation;

namespace Awacash.Application.Authentication.Handler.Commands.ResetAdminPassword
{
    public class ResetAdminPasswordCommandValidator : AbstractValidator<ResetAdminPasswordCommand>
    {
        public ResetAdminPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPasswprd).NotEmpty().NotNull().WithMessage("Confirm passwprd is required");
            RuleFor(x => x.ConfirmPasswprd).Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}

