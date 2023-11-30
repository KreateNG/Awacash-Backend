using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.RegisterkMobileDevice
{
    public record RegisterkMobileDeviceCommand(string Phone, string DeviceId, string Otp) :IRequest<ResponseModel<bool>>;
}
