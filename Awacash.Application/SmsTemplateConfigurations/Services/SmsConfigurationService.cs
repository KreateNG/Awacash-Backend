using System;
using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.SmsTemplateConfigurations.Services
{
    public class SmsConfigurationService : ISmsConfigurationService
    {
        private readonly ILogger<SmsConfigurationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public SmsConfigurationService(ILogger<SmsConfigurationService> logger, IUnitOfWork unitOfWork, ICurrentUser currentUser, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }
        public async Task<ResponseModel<SmsTemplateDto>> CreateSmsTemplateAsync(string message, SmsType smsType)
        {
            try
            {
                var smsConfig = await _unitOfWork.SmsTemplateRepository.GetByAsync(x => x.SmsType == smsType);
                if (smsConfig is not null) return ResponseModel<SmsTemplateDto>.Failure("Sms Configuration already exist");
                
                var smsTemplate = new SmsTemplate
                {
                    SmsType = smsType,
                    Message = message,
                    CreatedBy = _currentUser.GetCustomerId(),
                    CreatedDate = _dateTimeProvider.UtcNow,
                    CreatedByIp = "::1"
                };
                _unitOfWork.SmsTemplateRepository.Add(smsTemplate);
                await _unitOfWork.Complete();

                var SmsTemplateDto = _mapper.Map<SmsTemplateDto>(smsTemplate);
                return ResponseModel<SmsTemplateDto>.Success(SmsTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating sms template: {ex.Message}", nameof(CreateSmsTemplateAsync));
                return ResponseModel<SmsTemplateDto>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<SmsTemplateDto>>> GetAllSmsTemplateAsync()
        {
            try
            {
                var smsConfig = await _unitOfWork.SmsTemplateRepository.ListAllAsync();
                
                var SmsTemplateDto = _mapper.Map<List<SmsTemplateDto>>(smsConfig.ToList());
                return ResponseModel<List<SmsTemplateDto>>.Success(SmsTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating saving: {ex.Message}", nameof(GetAllSmsTemplateAsync));
                return ResponseModel<List<SmsTemplateDto>>.Failure("Exception error");
            }
        }

        public Task<ResponseModel<PagedResult<SmsTemplateDto>>> GetPaginatedSmsTemplateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<SmsTemplateDto>> GetSmsTemplateByIdAsync(string id)
        {
            try
            {
                var smsConfig = await _unitOfWork.SmsTemplateRepository.GetByIdAsync(id);
                if (smsConfig is not null) return ResponseModel<SmsTemplateDto>.Failure("Sms Configuration already exist");

                var SmsTemplateDto = _mapper.Map<SmsTemplateDto>(smsConfig);
                return ResponseModel<SmsTemplateDto>.Success(SmsTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating saving: {ex.Message}", nameof(GetSmsTemplateByIdAsync));
                return ResponseModel<SmsTemplateDto>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> UpdateSmsTemplateAsync(string id, string message)
        {
            try
            {
                var smsConfig = await _unitOfWork.SmsTemplateRepository.GetByAsync(x => x.Id == id);
                if (smsConfig is null) return ResponseModel<bool>.Failure("Sms Configuration not found");

                smsConfig.Message = message;
                smsConfig.ModifiedBy = _currentUser.GetCustomerId();
                smsConfig.ModifiedByIp = "::1";
                smsConfig.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.SmsTemplateRepository.Update(smsConfig);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updating sms template: {ex.Message}", nameof(UpdateSmsTemplateAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }
    }
}

