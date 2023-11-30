using Awacash.Application.Customers.Services;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.ChangePin
{
    public class ChangePinCommandHandler : IRequestHandler<ChangePinCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;

        public ChangePinCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<bool>> Handle(ChangePinCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.ChangePin(request.OldPin, request.NewPin);
        }
    }
}
