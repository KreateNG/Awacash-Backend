using FluentValidation;

namespace Awacash.Application.Authentication.Handler.Commands.Register;

public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
        RuleFor(x => x.Pin).NotEmpty().NotNull();
        RuleFor(x => x.LastName).NotEmpty().NotNull();
        RuleFor(x => x.FirstName).NotEmpty().NotNull();
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
    }
}
