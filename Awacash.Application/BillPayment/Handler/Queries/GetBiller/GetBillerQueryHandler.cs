using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Queries.GetBiller
{
    public class GetBillerQueryHandler : IRequestHandler<GetBillerQuery, ResponseModel<BillerCategory>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public GetBillerQueryHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        public async Task<ResponseModel<BillerCategory>> Handle(GetBillerQuery request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.GetBillerCatrgory();
        }
    }
}
