using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Queries.GetCustomerPaginatedTransactions
{
    public class GetCustomerPaginatedTransactionQuery:TransactionFilterModel, IRequest<ResponseModel<PagedResult<TransactionDTO>>>
    {
    }
}
