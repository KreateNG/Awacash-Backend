using System;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.AuditLogs.Handler.Queries.GetAuditLogById
{
    public class GetAuditLogByIdQueryHandler : IRequestHandler<GetAuditLogByIdQuery, ResponseModel<AuditLogDto>>
    {
        private readonly IAuditLogService _auditLogService;
        public GetAuditLogByIdQueryHandler(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task<ResponseModel<AuditLogDto>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
        {
            return await _auditLogService.GetAuditLogByIdAsync(request.Id);
        }
    }
}

