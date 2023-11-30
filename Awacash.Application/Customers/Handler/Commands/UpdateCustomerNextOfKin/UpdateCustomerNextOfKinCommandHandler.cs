using System;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Customers.Handler.Commands.UpdateCustomerNextOfKin
{
    public class UpdateCustomerNextOfKinCommandHandler : IRequestHandler<UpdateCustomerNextOfKinCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;
        public UpdateCustomerNextOfKinCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateCustomerNextOfKinCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.UpdateCustomerAddressAndNextOfKin(request.Address, request.State, request.City, request.NextOfKinName, request.NextOfKinRelationship, request.NextOfKinPhoneNumber, request.Country, request.NextOfKinEmail, request.NextOfKinAddress);
        }
    }
}

