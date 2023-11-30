using Awacash.Application.BillPayment.Handler.Queries.GetBiller;
using Awacash.Application.BillPayment.Services;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Queries.GetBillerCategory
{
    public class GetBillerCategoryQueryHandler : IRequestHandler<GetBillerCategoryQuery, ResponseModel<List<Biller>>>
    {
        private readonly IBillPaymentService _billPaymentService;

        public GetBillerCategoryQueryHandler(IBillPaymentService billPaymentService)
        {
            _billPaymentService = billPaymentService;
        }

        public async Task<ResponseModel<List<Biller>>> Handle(GetBillerCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _billPaymentService.GetBillerByCategory(request.CategoryId);
        }
    
    }
}
