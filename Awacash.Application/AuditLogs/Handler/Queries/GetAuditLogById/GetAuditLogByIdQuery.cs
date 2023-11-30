using System;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.AuditLogs.Handler.Queries.GetAuditLogById
{
    public record GetAuditLogByIdQuery(string Id) : IRequest<ResponseModel<AuditLogDto>>;
}

