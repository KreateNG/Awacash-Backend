using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.ValidateBvn
{
    public class ValidateBvnCommandHandler : IRequestHandler<ValidateBvnCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;
        public ValidateBvnCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<ResponseModel<bool>> Handle(ValidateBvnCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.ValidateCustomerBvn(request.FirstName, request.LastName, request.DateOfBirth, request.AccessToken);
        }
    }
}
