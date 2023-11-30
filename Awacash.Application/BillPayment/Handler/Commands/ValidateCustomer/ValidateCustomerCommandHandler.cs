using Awacash.Application.BillPayment.Handler.Commands.SendPaymentAdvice;
using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Commands.ValidateCustomer
{
    public class ValidateCustomerCommandHandler:IRequestHandler<ValidateCustomerCommand, ResponseModel<BillPaymentCustomer>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public ValidateCustomerCommandHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }


        public async Task<ResponseModel<BillPaymentCustomer>> Handle(ValidateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.ValidateCustomer(request.CustomerId, request.PaymentCode);
        }
    }
}
