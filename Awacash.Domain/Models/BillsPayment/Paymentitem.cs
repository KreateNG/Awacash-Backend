using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.BillsPayment
{
    public class Paymentitem
    {
        public string? Categoryid { get; set; }
        public string? Billerid { get; set; }
        public bool IsAmountFixed { get; set; }
        public string? Paymentitemid { get; set; }
        public string? Paymentitemname { get; set; }
        public string? Amount { get; set; }
        public string? Code { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? ItemCurrencySymbol { get; set; }
        public string? SortOrder { get; set; }
        public string? PictureId { get; set; }
        public string? PaymentCode { get; set; }
        public string? ItemFee { get; set; }
        public string? PaydirectItemCode { get; set; }
    }
}
