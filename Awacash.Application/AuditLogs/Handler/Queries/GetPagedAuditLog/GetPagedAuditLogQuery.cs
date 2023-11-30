using System;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;

namespace Awacash.Application.AuditLogs.Handler.Queries.GetPagedAuditLog
{
    public class GetPagedAuditLogQuery : AuditLogFilterModel, IRequest<ResponseModel<PagedResult<AuditLogDto>>>
    {

    }
}

