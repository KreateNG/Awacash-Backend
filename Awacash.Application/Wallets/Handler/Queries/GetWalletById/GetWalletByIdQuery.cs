using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetWalletById
{
    public record GetWalletByIdQuery(string Id) : IRequest<ResponseModel<WalletDTO>>;
}

