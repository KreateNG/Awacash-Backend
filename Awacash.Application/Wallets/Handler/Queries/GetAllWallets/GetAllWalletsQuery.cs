using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetAllWallets
{

    public record GetAllWalletsQuery() : IRequest<ResponseModel<List<WalletDTO>>>;
}

