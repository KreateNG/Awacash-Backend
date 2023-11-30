using Awacash.Application.Common.Interfaces.Authentication;
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

namespace Awacash.Application.Transactions.Handlers.Queries.GetCustomerPaginatedTransactions
{
    public class GetCustomerPaginatedTransactionQueryHandler : IRequestHandler<GetCustomerPaginatedTransactionQuery, ResponseModel<PagedResult<TransactionDTO>>>
    {
        private readonly ITransactionService _transactionService;
        private readonly ICurrentUser _currentUser;

        public GetCustomerPaginatedTransactionQueryHandler(ITransactionService transactionService, ICurrentUser currentUser)
        {
            _transactionService = transactionService;
            _currentUser = currentUser;
        }
        public async Task<ResponseModel<PagedResult<TransactionDTO>>> Handle(GetCustomerPaginatedTransactionQuery request, CancellationToken cancellationToken)
        {
            request.CustomerId = _currentUser.GetCustomerId();
            return await _transactionService.GetPaginatedTransactions(request);
        }
    }
}
