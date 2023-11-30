using System;
using System.Linq.Expressions;
using Awacash.Domain.Entities;
using Awacash.Domain.Specifications;

namespace Awacash.Application.AuditLogs.Specifications
{
    public class AuditLogFilterSpecification : BaseSpecification<AuditLog>
    {
        public AuditLogFilterSpecification(string? eventType, string? tableName) :
            base(
                x =>
                (string.IsNullOrWhiteSpace(eventType) || x.EventType.ToLower() == eventType.ToLower()) &&
                (string.IsNullOrWhiteSpace(tableName) || x.TableName.ToLower() == tableName.ToLower())
            )
        {
        }
    }
}

