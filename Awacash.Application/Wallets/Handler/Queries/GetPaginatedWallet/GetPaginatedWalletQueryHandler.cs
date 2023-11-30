using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetPaginatedWallet
{
    public class GetPaginatedWalletQueryHandler : IRequestHandler<GetPaginatedWalletQuery, ResponseModel<PagedResult<WalletDTO>>>
    {
        private readonly IWalletService _walletService;
        public GetPaginatedWalletQueryHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ResponseModel<PagedResult<WalletDTO>>> Handle(GetPaginatedWalletQuery request, CancellationToken cancellationToken)
        {
            return await _walletService.GetPaginatedWalletAsync(request);
        }
    }
}

