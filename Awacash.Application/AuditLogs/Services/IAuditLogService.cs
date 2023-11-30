using System;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;

namespace Awacash.Application.AuditLogs.Services
{
    public interface IAuditLogService
    {
        Task<ResponseModel<PagedResult<AuditLogDto>>> GetPagedAuditLogAsync(AuditLogFilterModel filterModel);
        Task<ResponseModel<AuditLogDto>> GetAuditLogByIdAsync(string id);
    }
}

