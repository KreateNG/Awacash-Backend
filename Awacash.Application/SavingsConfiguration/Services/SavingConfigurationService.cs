using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Savings.Services;
using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.SavingsConfiguration.Services
{
    public class SavingConfigurationService : ISavingConfigurationService
    {
        private readonly ILogger<SavingService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public SavingConfigurationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<SavingService> logger, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }
        public async Task<ResponseModel<SavingConfigurationDTO>> CreateSavingConfigurationAsync(string planName, string planDescription, int planDuration, decimal planInterestRate, SavingType savingType, string productCode)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(_currentUser.GetUserId().ToString());
                if (user is null) return ResponseModel<SavingConfigurationDTO>.Failure("User not found");

                var savingConfiguration = new SavingConfiguration
                {
                    PlanName = planName,
                    PlanDescription = planDescription,
                    PlanDuration = planDuration,
                    PlanInterestRate = planInterestRate,
                    SavingType = savingType,
                    Status = true,
                    CreatedBy = _currentUser.GetUserEmail(),
                    CreatedByIp = "::1",
                    CreatedDate = _dateTimeProvider.UtcNow,
                    ProductCode = productCode
                };

                _unitOfWork.SavingConfigurationRepository.Add(savingConfiguration);
                await _unitOfWork.Complete();
                return ResponseModel<SavingConfigurationDTO>.Success(_mapper.Map<SavingConfigurationDTO>(savingConfiguration));

            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while creating saving configuration: {ex.Message}", nameof(CreateSavingConfigurationAsync));
                return ResponseModel<SavingConfigurationDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<SavingConfigurationDTO>>> GetAllSavingConfigurationAsync()
        {
            try
            {
                var savingsConfigs = await _unitOfWork.SavingConfigurationRepository.ListAllAsync();
                return ResponseModel<List<SavingConfigurationDTO>>.Success(_mapper.Map<List<SavingConfigurationDTO>>(savingsConfigs));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting all saving configuration: {ex.Message}", nameof(GetAllSavingConfigurationAsync));
                return ResponseModel<List<SavingConfigurationDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<SavingConfigurationDTO>> GetSavingConfigurationByIdAsync(string id)
        {
            try
            {
                var savingsConfig = await _unitOfWork.SavingConfigurationRepository.GetByIdAsync(id);
                return ResponseModel<SavingConfigurationDTO>.Success(_mapper.Map<SavingConfigurationDTO>(savingsConfig));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting saving configuration by id: {ex.Message}", nameof(GetAllSavingConfigurationAsync));
                return ResponseModel<SavingConfigurationDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<SavingConfigurationDTO>> UpdateSavingConfigurationAsync(string id, string planName, string planDescription, int planDuration, decimal planInterestRate, SavingType savingType)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(_currentUser.GetUserId().ToString());
                if (user is null) return ResponseModel<SavingConfigurationDTO>.Failure("User not found");

                var savingConfiguration = await _unitOfWork.SavingConfigurationRepository.GetByIdAsync(id);
                if (savingConfiguration is null) return ResponseModel<SavingConfigurationDTO>.Failure("Saving configuration not found");

                savingConfiguration.PlanName = planName;
                savingConfiguration.PlanDescription = planDescription;
                savingConfiguration.PlanDuration = planDuration;
                savingConfiguration.PlanInterestRate = planInterestRate;
                savingConfiguration.SavingType = savingType;
                savingConfiguration.ModifiedBy = _currentUser.GetUserEmail();
                savingConfiguration.ModifiedByIp = "::1";
                savingConfiguration.ModifiedDate = _dateTimeProvider.UtcNow;


                _unitOfWork.SavingConfigurationRepository.Update(savingConfiguration);
                await _unitOfWork.Complete();
                return ResponseModel<SavingConfigurationDTO>.Success(_mapper.Map<SavingConfigurationDTO>(savingConfiguration));

            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while creating saving configuration: {ex.Message}", nameof(CreateSavingConfigurationAsync));
                return ResponseModel<SavingConfigurationDTO>.Failure("Exception error");
            }
        }
    }
}
