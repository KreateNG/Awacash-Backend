using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.RegisterkMobileDevice
{
    public class RegisterkMobileDeviceCommandHandler : IRequestHandler<RegisterkMobileDeviceCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;

        public RegisterkMobileDeviceCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<bool>> Handle(RegisterkMobileDeviceCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.RegisterkMobileDevice(request.Phone, request.DeviceId, request.Otp);
        }
    }
}
