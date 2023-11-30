using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Settings
{
    public class AppSettings
    {
        public const string SectionName = "AppSettings";
        public string? UserProfileFilePath { get; set; }
        public string? SystemPath { get; set; }
        public string? ProfilePath { get; set; }
        public string? PromotionPath { get; set; }
        public string? DomainName { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiId { get; set; }
        public string? CreditGL { get; set; }
        public string? DebitGL { get; set; }
        public string? SmsAccountNumber { get; set; }
        public string? SmsAccountId { get; set; }
    }
}
