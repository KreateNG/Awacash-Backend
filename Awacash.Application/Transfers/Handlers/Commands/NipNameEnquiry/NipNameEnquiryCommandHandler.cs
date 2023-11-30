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

namespace Awacash.Application.Transfers.Handlers.Commands.NipNameEnquiry
{
    public class NipNameEnquiryCommandHandler : IRequestHandler<NipNameEnquiryCommand, ResponseModel<NipNameEnquiryResponse>>
    {
        private readonly ITransferService _transferService;

        public NipNameEnquiryCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }
        public async Task<ResponseModel<NipNameEnquiryResponse>> Handle(NipNameEnquiryCommand request, CancellationToken cancellationToken)
        {
            return await _transferService.PostNipNameEnquiry(request.AccountNumber, request.BankCode);
        }
    }
}
