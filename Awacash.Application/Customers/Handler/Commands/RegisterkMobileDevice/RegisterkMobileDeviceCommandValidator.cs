using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.RegisterkMobileDevice
{
    public class RegisterkMobileDeviceCommandValidator: AbstractValidator<RegisterkMobileDeviceCommand>
    {
        public RegisterkMobileDeviceCommandValidator()
        {
            RuleFor(x => x.Otp).NotEmpty().NotNull().WithMessage("OTP is required");
            RuleFor(x => x.Phone).NotEmpty().NotNull().WithMessage("Phone number is required");
            RuleFor(x => x.DeviceId).NotEmpty().NotNull().WithMessage("Device ID is required");
        }
    }
}
