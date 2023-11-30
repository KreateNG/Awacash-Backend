using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Queries.GetPaginatedTransactions
{
    public class GetPaginatedTransactionQueryHandler : IRequestHandler<GetPaginatedTransactionQuery, ResponseModel<PagedResult<TransactionDTO>>>
    {
        private readonly ITransactionService _transactionService;
        public GetPaginatedTransactionQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<ResponseModel<PagedResult<TransactionDTO>>> Handle(GetPaginatedTransactionQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.GetPaginatedTransactions(request);
        }
    }
}
