using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, ResponseModel<WalletDTO>>
    {
        private readonly IWalletService _walletService;
        public GetWalletByIdQueryHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ResponseModel<WalletDTO>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            return await _walletService.GetWalletByIdAsync(request.Id);
        }
    }
}

