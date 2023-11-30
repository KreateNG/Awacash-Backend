using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.UploadProfile
{
    public class UploadProfileCommandHandler : IRequestHandler<UploadProfileCommand, ResponseModel<string>>
    {
        private readonly ICustomerService _customerService;

        public UploadProfileCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<string>> Handle(UploadProfileCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.UploadCustomerProfileImageAsync(request.PasswordBase64);
        }
    }
}
