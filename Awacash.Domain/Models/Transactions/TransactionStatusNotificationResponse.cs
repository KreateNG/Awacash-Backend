using System;
namespace Awacash.Domain.Models.Transactions
{
    public record TransactionStatusNotificationResponse(string Status, string Status_desc, List<TransactionQuery> Transactions);


    public class TransactionQuery
    {
        public string? Originatoraccountnumber { get; set; }
        public string? Amount { get; set; }
        public string? Originatorname { get; set; }
        public string? Narration { get; set; }
        public string? Craccountname { get; set; }
        public string? Paymentreference { get; set; }
        public string? Bankname { get; set; }
        public string? Sessionid { get; set; }
        public string? Craccount { get; set; }
        public string? Bankcode { get; set; }
        public string? Requestdate { get; set; }
        public string? Nibssresponse { get; set; }
        public string? Sendstatus { get; set; }
        public string? Sendresponse { get; set; }
    }
}

