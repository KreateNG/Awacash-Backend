using Awacash.Application.Customers.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.PoneVerification
{
    public class SendPhoneVerificationCodeCommandHandler : IRequestHandler<SendPhoneVerificationCodeCommand, ResponseModel<string>>
    {
        private readonly ICustomerService _customerService;

        public SendPhoneVerificationCodeCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponseModel<string>> Handle(SendPhoneVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.SendPhoneNumberVerificationCode(phoneNumber: request.PhoneNumber);
        }
    }
}
