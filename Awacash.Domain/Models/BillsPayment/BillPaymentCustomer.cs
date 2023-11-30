using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.BillsPayment
{
    public class BillPaymentCustomer
    {
        public string? PaymentCode { get; set; }
        public string? CustomerId { get; set; }
        public string? ResponseCode { get; set; }
        public string? FullName { get; set; }
        public string? Amount { get; set; }
        public string? AmountType { get; set; }
        public string? AmountTypeDescription { get; set; }
        public string? ResponseDescription { get; set; }
    }
}
