using System;
using Awacash.Application.Wallets.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Commands.UpdateWalletStatus
{
    public class UpdateWalletStatusCommandHandler : IRequestHandler<UpdateWalletStatusCommand, ResponseModel<bool>>
    {
        private readonly IWalletService _walletService;
        public UpdateWalletStatusCommandHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateWalletStatusCommand request, CancellationToken cancellationToken)
        {
            return await _walletService.UpdateWalletStattusAsync(request.Id);
        }
    }
}

