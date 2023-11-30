using System;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.AuditLogs.Handler.Queries.GetPagedAuditLog
{
    public class GetPagedAuditLogQueryHandler : IRequestHandler<GetPagedAuditLogQuery, ResponseModel<PagedResult<AuditLogDto>>>
    {
        private readonly IAuditLogService _auditLogService;
        public GetPagedAuditLogQueryHandler(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task<ResponseModel<PagedResult<AuditLogDto>>> Handle(GetPagedAuditLogQuery request, CancellationToken cancellationToken)
        {
            return await _auditLogService.GetPagedAuditLogAsync(request);
        }
    }
}

