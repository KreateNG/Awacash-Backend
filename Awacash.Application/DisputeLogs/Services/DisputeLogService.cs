using AutoMapper;
using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Beneficiaries.Services;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.DisputeLogs.DTOs;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.DisputeLogs.Services
{
    public class DisputeLogService : IDisputeLogService
    {
        private readonly ILogger<DisputeLogService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAwacashThirdPartyService _berachahThirdPartyService;

        public DisputeLogService(ILogger<DisputeLogService> logger, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser, IDateTimeProvider dateTimeProvider, IAwacashThirdPartyService berachahThirdPartyService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _berachahThirdPartyService = berachahThirdPartyService;
        }
        public async Task<ResponseModel<bool>> CreateDisputeLogAsync(string accountNumber, string email, string phoneNumber, decimal amount, DateTime transactionDate, string comment)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return ResponseModel<bool>.Failure("Idnetifier is requires");
                }
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure("Customer not found");
                }

                var disputeLog = new DisputeLog
                {
                    AccountNumber = accountNumber,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Amount = amount,
                    TransactionDate = transactionDate,
                    Comment = comment,
                    CreatedBy = _currentUser.Name,
                    CreatedDate = _dateTimeProvider.UtcNow,
                    CreatedByIp = "::1",
                    IsDeleted = false,
                    CustomerId = customerId,
                };
                _unitOfWork.DispusteLogRepository.Add(disputeLog);
                await _unitOfWork.Complete();
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.ComplaintResolution);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[REASON].", comment));
                }
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateDisputeLogAsync));
                return ResponseModel<bool>.Failure($"error occured while creating dispute log");
            }
        }

        public async Task<ResponseModel<List<DisputeLogDTO>>> GetAllDisputeLogAsync()
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var disputeLogs = await _unitOfWork.DispusteLogRepository.ListAllAsync();
                return ResponseModel<List<DisputeLogDTO>>.Success(_mapper.Map<List<DisputeLogDTO>>(disputeLogs.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting dispute logs: {ex.Message}", nameof(GetAllDisputeLogAsync));
                return ResponseModel<List<DisputeLogDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<DisputeLogDTO>> GetDisputeLogByIdAsync(string id)
        {
            try
            {
                var cardRequest = await _unitOfWork.DispusteLogRepository.GetByAsync(x => x.Id == id);
                return ResponseModel<DisputeLogDTO>.Success(_mapper.Map<DisputeLogDTO>(cardRequest));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(GetDisputeLogByIdAsync));
                return ResponseModel<DisputeLogDTO>.Failure($"error occured while fetching dispute log");
            }
        }

        public Task<ResponseModel<PagedResult<DisputeLogDTO>>> GetPagedDisputeLogAsync()
        {
            throw new NotImplementedException();
        }
    }
}
