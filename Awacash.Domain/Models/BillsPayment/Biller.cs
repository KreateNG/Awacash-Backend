using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.BillsPayment
{
    public class Biller
    {
        public string? Categoryid { get; set; }
        public string? Categoryname { get; set; }
        public string? Categorydescription { get; set; }
        public string? Billerid { get; set; }
        public string? Billername { get; set; }
        public string? Customerfield1 { get; set; }
        public string? Customerfield2 { get; set; }
        public string? Supportemail { get; set; }
        public string? PaydirectProductId { get; set; }
        public string? PaydirectInstitutionId { get; set; }
        public string? Narration { get; set; }
        public string? ShortName { get; set; }
        public string? Surcharge { get; set; }
        public string? CurrencyCode { get; set; }
        public string? QuickTellerSiteUrlName { get; set; }
        public string? AmountType { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? CustomSectionUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public string? RiskCategoryId { get; set; }
        public string? NetworkId { get; set; }
        public string? ProductCode { get; set; }
    }

    public record Category(string? Categoryid, string? Categoryname, string? Categorydescription);

    public record BillerCategory(List<Category>? categorys);
}
