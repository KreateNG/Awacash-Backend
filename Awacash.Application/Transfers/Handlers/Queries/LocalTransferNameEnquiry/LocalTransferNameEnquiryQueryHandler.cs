using Awacash.Application.Transactions.Services;
using Awacash.Application.Transfers.Services;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Queries.LocalTransferNameEnquiry
{
    internal class LocalTransferNameEnquiryQueryHandler : IRequestHandler<LocalTransferNameEnquiryQuery, ResponseModel<NipNameEnquiryResponse>>
    {
        private readonly ITransferService _transferService;

        public LocalTransferNameEnquiryQueryHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public async Task<ResponseModel<NipNameEnquiryResponse>> Handle(LocalTransferNameEnquiryQuery request, CancellationToken cancellationToken)
        {
            return await _transferService.LocalTransferNameEnquiry(request.PhoneNumber);
        }
    }
}
