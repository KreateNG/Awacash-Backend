using System;
using Awacash.Domain.FilterModels;

namespace Awacash.Application.AuditLogs.FilterModels
{
    public class AuditLogFilterModel : BaseFilterModel
    {
        public string? EventType { get; set; }

        public string? TableName { get; set; }
    }
}

