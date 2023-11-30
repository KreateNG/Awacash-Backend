using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Commands.UpdateWalletStatus
{
    public record UpdateWalletStatusCommand(string Id) : IRequest<ResponseModel<bool>>;
}

