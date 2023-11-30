using System;
using Awacash.Application.Common.Model;

namespace AwaCash.Application.Common.Model
{
    public class OtpResponseDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public bool IsUsed { get; set; } = false!;
        public DateTime? ExpiresAt { get; set; }
    }



    public class OtpRequestModel
    {
        public string ResourceId { get; set; } = default!;
        public string ResourceName { get; set; } = default!;
    }

    public class OtpUpdateRequestModel
    {
        public string Hash { get; set; } = default!;
    }
}

