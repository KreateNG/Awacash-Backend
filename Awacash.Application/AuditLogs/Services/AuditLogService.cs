using System;
using AutoMapper;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.FilterModels;
using Awacash.Application.AuditLogs.Specifications;
using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Beneficiaries.Services;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Specifications;
using Awacash.Domain.FilterModels;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.AuditLogs.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuditLogService> _logger;
        private readonly IMapper _mapper;
        public AuditLogService(IUnitOfWork unitOfWork, ILogger<AuditLogService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseModel<AuditLogDto>> GetAuditLogByIdAsync(string id)
        {
            try
            {
                var auditLog = await _unitOfWork.AuditLogRepository.GetByAsync(x => x.Id == id);
                return ResponseModel<AuditLogDto>.Success(_mapper.Map<AuditLogDto>(auditLog));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting beneficary by id: {ex.Message}", nameof(GetAuditLogByIdAsync));
                return ResponseModel<AuditLogDto>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PagedResult<AuditLogDto>>> GetPagedAuditLogAsync(AuditLogFilterModel filterModel)
        {
            try
            {
                var auditLogSpecification = new AuditLogFilterSpecification(tableName: filterModel.TableName, eventType: filterModel.EventType);
                var auditLogs = await _unitOfWork.AuditLogRepository.ListAsync(filterModel.PageIndex, filterModel.PageSize, auditLogSpecification);
                return ResponseModel<PagedResult<AuditLogDto>>.Success(_mapper.Map<PagedResult<AuditLogDto>>(auditLogs));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetPagedAuditLogAsync));
                return ResponseModel<PagedResult<AuditLogDto>>.Failure("Exception error");
            }
        }
    }
}

