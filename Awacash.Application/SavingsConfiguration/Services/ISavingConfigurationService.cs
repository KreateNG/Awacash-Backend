using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Domain.Enums;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Services
{
    public interface ISavingConfigurationService
    {
        Task<ResponseModel<SavingConfigurationDTO>> CreateSavingConfigurationAsync(string planName, string planDescription, int planDuration, decimal planInterestRate, SavingType savingType, string productCode);
        Task<ResponseModel<SavingConfigurationDTO>> UpdateSavingConfigurationAsync(string id, string planName, string planDescription, int planDuration, decimal planInterestRate, SavingType savingType);
        Task<ResponseModel<SavingConfigurationDTO>> GetSavingConfigurationByIdAsync(string id);
        Task<ResponseModel<List<SavingConfigurationDTO>>> GetAllSavingConfigurationAsync();
    }
}
