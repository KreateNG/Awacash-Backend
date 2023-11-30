using Awacash.Application.Customers.Services;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponseModel<bool>>
    {
        private readonly ICustomerService _customerService;

        public ChangePasswordCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.ChangePassword(request.OldPassword, request.newPassword);
        }
    }
}
