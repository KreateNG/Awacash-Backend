using System;
using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.BillPayment.Handler.Commands.AirTimePurchase
{
    public class AirTimePurchaseCommandHandler : IRequestHandler<AirTimePurchaseCommand, ResponseModel<PaymentAdviceResponse>>
    {
        private readonly IBillPaymentService _billPaymentService;
        public AirTimePurchaseCommandHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }
        public async Task<ResponseModel<PaymentAdviceResponse>> Handle(AirTimePurchaseCommand request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.AirtimePurchase(request.AccountNumber, request.PaymentCode, request.CustomerMobile, request.Amount, request.Pin);
        }
    }



    public class WalletAirTimePurchaseCommandHandler : IRequestHandler<WalletAirTimePurchaseCommand, ResponseModel<PaymentAdviceResponse>>
    {
        private readonly IBillPaymentService _billPaymentService;
        public WalletAirTimePurchaseCommandHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }
        public async Task<ResponseModel<PaymentAdviceResponse>> Handle(WalletAirTimePurchaseCommand request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.WalletAirtimePurchase(request.PaymentCode, request.CustomerMobile, request.Amount, request.Pin);
        }
    }
}

