using Awacash.Application.Transactions.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Queries.GetCustomerTransactions
{
    public record GetCustomerTransactionsQuery():IRequest<ResponseModel<List<TransactionDTO>>>;
}
