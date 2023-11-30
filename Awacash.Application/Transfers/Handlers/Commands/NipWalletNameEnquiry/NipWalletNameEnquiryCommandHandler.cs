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

namespace Awacash.Application.Transfers.Handlers.Commands.NipWalletNameEnquiry
{
    public class NipNameEnquiryCommandHandler : IRequestHandler<NipWalletNameEnquiryCommand, ResponseModel<NipNameEnquiryResponse>>
    {
        private readonly ITransferService _transferService;

        public NipNameEnquiryCommandHandler(ITransferService transferService)
        {
            _transferService = transferService;
        }
        public async Task<ResponseModel<NipNameEnquiryResponse>> Handle(NipWalletNameEnquiryCommand request, CancellationToken cancellationToken)
        {
            return await _transferService.PostWalletNipNameEnquiry(request.AccountNumber, request.BankCode);
        }
    }
}
