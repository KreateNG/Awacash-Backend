using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Awacash.Domain.Entities
{
    public class Document : BaseEntity
    {
        public Document()
        {
        }
        public string? ResourceUrl { get; set; }
        public string? IDNumber { get; set; }
        public FileType? FileType { get; set; }
        public Customer Customer { get; set; } = default!;
        public int VerificationTrial { get; set; }
        public StatusEnum? Status { get; set; }
    }

    public enum StatusEnum
    {
        Pending = 0,
        Processing,
        Success,
        Failed
    }

    public enum FileType
    {
        [Description("NIN")]
        NIN = 1,
        [Description("Drivers license")]
        DRIVERS_LICENSE,
        [Description("International passport")]
        PASSPORT,
        [Description("Voters card")]
        VOTERS_CARD,
        [Description("Others")]
        OTHERS
    }
}

