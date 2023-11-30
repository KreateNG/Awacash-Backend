using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Specifications
{
    public class TransactionFilterSpecification : BaseSpecification<Transaction>
    {
        public TransactionFilterSpecification(string customerId, DateTime? startDate, DateTime? endDate, TransactionType? transactionType, RecordType? recordType, string accountNumber, string orderBy, bool? byDescending) :
            base(
                x =>
                (string.IsNullOrWhiteSpace(accountNumber) || x.CreditAccountNumber == accountNumber || x.DebitAccountNumber == accountNumber)&&
                (string.IsNullOrWhiteSpace(customerId) || x.CustomerId == customerId) &&
                (!startDate.HasValue || x.CreatedDate >= startDate) &&
                (!endDate.HasValue || x.CreatedDate <= endDate) &&
                (!transactionType.HasValue || x.TransactionType == transactionType) &&
                (!recordType.HasValue || x.RecordType == recordType)
            )
        {
            if (!string.IsNullOrWhiteSpace(orderBy) && byDescending.HasValue)
            {
                switch (orderBy.ToLower())
                {
                    case "createddate":
                        {
                            if (byDescending.Value)
                                ApplyOrderByDescending(x => x.CreatedDate);
                            else
                                ApplyOrderBy(x => x.CreatedDate);

                            break;
                        }
                }
            }
        }
    }
}
