using Awacash.Application.BillPayment.Handler.Queries.GetBillerCategory;
using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Queries.GetBillerPaymentItem
{
    public class GetBillerPaymentItemQueryHandler : IRequestHandler<GetBillerPaymentItemQuery, ResponseModel<List<Paymentitem>>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public GetBillerPaymentItemQueryHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        public async Task<ResponseModel<List<Paymentitem>>> Handle(GetBillerPaymentItemQuery request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.GetBillerPaymentItems(request.BillerId);
        }
    }
}
