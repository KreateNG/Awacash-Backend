

using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.PoneVerification
{
    public class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, ResponseModel<string>>
    {
        private readonly ICustomerService _customerService;

        public VerifyPhoneCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Task<ResponseModel<string>> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
        {
            return _customerService.VerificationPhoneNumber(request.Code, request.Hash);
        }
    }
}
