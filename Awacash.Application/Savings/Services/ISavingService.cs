using Awacash.Application.Savings.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Services
{
    public interface ISavingService
    {
        Task<ResponseModel<SavingDTO>> CreateSavingAsync(string reason, decimal targetAmount, DeductionFrequency deductionFrequency, string savingConfigId, SavingsDuration SavingsDuration, string accountNumber);
        Task<ResponseModel<List<SavingDTO>>> GetAllSavingsAsync();
        Task<ResponseModel<List<SavingDTO>>> GetAllSavingsByIdAsync();
        Task<ResponseModel<SavingDTO>> GetSavingByIdAsync(string id);
        Task<ResponseModel<PagedResult<SavingDTO>>> GetPaginatedSavingAsync();
    }
}
