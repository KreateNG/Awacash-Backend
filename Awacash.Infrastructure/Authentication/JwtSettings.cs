using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string? Secret { get; set; }
        public long ExpiratryMinutes { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }


}
