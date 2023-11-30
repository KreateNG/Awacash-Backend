using System;
namespace Awacash.Application.AuditLogs.DTOs
{
    public class AuditLogDto
    {
        public string? IPAddress { get; set; }

        public string? UserName { get; set; }

        public DateTime? EventDate { get; set; }


        public string? EventType { get; set; }

        public string? TableName { get; set; }

        public string? KeyValues { get; set; }

        public string? OldValues { get; set; }

        public string? Changes { get; set; }

        public string? ColumnValues { get; set; }

        public string? NewValues { get; set; }
    }
}

