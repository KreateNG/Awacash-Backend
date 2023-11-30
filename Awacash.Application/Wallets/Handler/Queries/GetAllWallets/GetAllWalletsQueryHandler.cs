using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetAllWallets
{
    public class GetAllWalletsQueryHandler : IRequestHandler<GetAllWalletsQuery, ResponseModel<List<WalletDTO>>>
    {
        private readonly IWalletService _walletService;

        public GetAllWalletsQueryHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ResponseModel<List<WalletDTO>>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
        {
            return await _walletService.GetAllWalletAsync();
        }
    }
}

