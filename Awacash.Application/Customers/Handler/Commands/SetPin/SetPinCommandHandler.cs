using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.Register;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.SetPin
{
    public class SetPinCommandHandler : IRequestHandler<SetPinCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;

        public SetPinCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Task<ResponseModel<bool>> Handle(SetPinCommand request, CancellationToken cancellationToken)
        {
            return _customerService.SetPin(request.Pin);
        }
    }
}
