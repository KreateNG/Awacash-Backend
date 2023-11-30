using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.FeeConfigurations.Services
{
    public interface IFeeConfigurationService
    {
        Task<ResponseModel<bool>> CreateFeeConfigurationAsync(TransactionType? transactionType, decimal? upperBound, decimal? lowerBound, decimal? fee);
        Task<ResponseModel<bool>> UpdateFeeConfigurationAsync(string id, TransactionType transactionType, decimal upperBound, decimal lowerBound, decimal fee);
        Task<ResponseModel<decimal>> GetTransactionFeeAsync(decimal amount, TransactionType transactionType);
        Task<ResponseModel<FeeConfigurationDto>> GetFeeConfigurationByIdAsync(string id);
        Task<ResponseModel<List<FeeConfigurationDto>>> GetAllFeeConfigurationAsync();
    }
}
