using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string? CustomerId { get; set; }
        public string? DebitAccountNumber { get; set; }
        public string? DebitAccountName { get; set; }
        public string? DestinationBankCode { get; set; }
        public string? DestinationBankName { get; set; }
        public string? CreditAccountNumber { get; set; }
        public string? CreditAccountName { get; set; }
        public TransactionType? TransactionType { get; set; }
        public string? TransactionReference { get; set; }
        public string? BillerId { get; set; }
        public string? BillerName { get; set; }
        public string? PaymentItemName { get; set; }
        public string? PaymentItemCode { get; set; }
        public string? Narration { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal? Fee { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentReference { get; set; }
        public string? Status { get; set; }
        public string? ResponseCode { get; set; }
        public RecordType? RecordType { get; set; }
        public string? RechargePIN { get; set; }
        public string? CustomerAddress { get; set; }

    }
}
