using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.Wallets.Handler.Queries.GetPaginatedWallet
{
    public class GetPaginatedWalletQuery : WalletFilterModel, IRequest<ResponseModel<PagedResult<WalletDTO>>> { }
}

