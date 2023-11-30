using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Commands.SendPaymentAdvice
{
    public class SendPaymentAdviceCommandHandler : IRequestHandler<SendPaymentAdviceCommand, ResponseModel<PaymentAdviceResponse>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public SendPaymentAdviceCommandHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> Handle(SendPaymentAdviceCommand request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.SendPaymentAdvice(request.AccountNumber, request.PaymentCode, request.CustomerId, request.CustomerMobile, request.CustomerEmail, request.Amount, request.Pin);
        }
    }


    public class SendWalletPaymentAdviceCommandHandler : IRequestHandler<SendWalletPaymentAdviceCommand, ResponseModel<PaymentAdviceResponse>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public SendWalletPaymentAdviceCommandHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> Handle(SendWalletPaymentAdviceCommand request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.SendWalletPaymentAdvice(request.PaymentCode, request.CustomerId, request.CustomerMobile, request.CustomerEmail, request.Amount, request.Pin);
        }
    }
}
