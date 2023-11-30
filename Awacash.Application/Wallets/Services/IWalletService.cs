using System;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;

namespace Awacash.Application.Wallets.Services
{
    public interface IWalletService
    {
        Task<ResponseModel<List<WalletDTO>>> GetAllWalletAsync();
        Task<ResponseModel<PagedResult<WalletDTO>>> GetPaginatedWalletAsync(WalletFilterModel walletFilterModel);
        Task<ResponseModel<WalletDTO>> GetWalletByIdAsync(string Id);
        Task<ResponseModel<bool>> UpdateWalletStattusAsync(string Id);
    }
}

