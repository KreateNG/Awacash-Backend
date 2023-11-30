using System;
using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Services;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.EmailTemplateConfigurations.Services
{
    public class EmailConfigurationService : IEmailConfigurationService
    {
        private readonly ILogger<EmailConfigurationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public EmailConfigurationService(ILogger<EmailConfigurationService> logger, IUnitOfWork unitOfWork, ICurrentUser currentUser, IDateTimeProvider dateTimeProvider, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }
        public async Task<ResponseModel<EmailTemplateDto>> CreateEmailTemplateAsync(string body, string subject, EmailType emailType)
        {
            try
            {
                var emailConfig = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == emailType);
                if (emailConfig is not null) return ResponseModel<EmailTemplateDto>.Failure("Email Configuration already exist");
                
                var emailTemplate = new EmailTemplate
                {
                    EmailType = emailType,
                    Body = body,
                    Subject = subject,
                    CreatedBy = _currentUser.GetCustomerId(),
                    CreatedDate = _dateTimeProvider.UtcNow,
                    CreatedByIp = "::1"
                };
                _unitOfWork.EmailTemplateRepository.Add(emailTemplate);
                await _unitOfWork.Complete();

                var SmsTemplateDto = _mapper.Map<EmailTemplateDto>(emailTemplate);
                return ResponseModel<EmailTemplateDto>.Success(SmsTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating email template: {ex.Message}", nameof(CreateEmailTemplateAsync));
                return ResponseModel<EmailTemplateDto>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<EmailTemplateDto>>> GetAllEmailTemplateAsync()
        {
            try
            {
                var emailConfig = await _unitOfWork.EmailTemplateRepository.ListAllAsync();
                
                var emailTemplateDto = _mapper.Map<List<EmailTemplateDto>>(emailConfig.ToList());
                return ResponseModel<List<EmailTemplateDto>>.Success(emailTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating email template: {ex.Message}", nameof(GetAllEmailTemplateAsync));
                return ResponseModel<List<EmailTemplateDto>>.Failure("Exception error");
            }
        }

        public Task<ResponseModel<PagedResult<EmailTemplateDto>>> GetPaginatedEmailTemplateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<EmailTemplateDto>> GetEmailTemplateByIdAsync(string id)
        {
            try
            {
                var smsConfig = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id);
                if (smsConfig is not null) return ResponseModel<EmailTemplateDto>.Failure("Email Configuration already exist");

                var emailTemplateDto = _mapper.Map<EmailTemplateDto>(smsConfig);
                return ResponseModel<EmailTemplateDto>.Success(emailTemplateDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating saving: {ex.Message}", nameof(GetEmailTemplateByIdAsync));
                return ResponseModel<EmailTemplateDto>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<bool>> UpdateEmailTemplateAsync(string id, string body, string subject)
        {
            try
            {
                var emailConfig = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.Id == id);
                if (emailConfig is null) return ResponseModel<bool>.Failure("Email Configuration not found");

                emailConfig.Body = body;
                emailConfig.Subject = subject;
                emailConfig.ModifiedBy = _currentUser.GetCustomerId();
                emailConfig.ModifiedByIp = "::1";
                emailConfig.ModifiedDate = _dateTimeProvider.UtcNow;
                _unitOfWork.EmailTemplateRepository.Update(emailConfig);
                await _unitOfWork.Complete();
                return ResponseModel<bool>.Success(true);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updating email template: {ex.Message}", nameof(UpdateEmailTemplateAsync));
                return ResponseModel<bool>.Failure("Exception error");
            }
        }
    }
}

