using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Queries.Login
{
    public class LoginQueryValidator: AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
            //RuleFor(x => x.DeviceId).NotEmpty().NotNull().WithMessage("Device ID is required");
        }
    }
}
