using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Queries.GetCustomerTransactions
{
    public class GetCustomerTransactionsQueryHandler : IRequestHandler<GetCustomerTransactionsQuery, ResponseModel<List<TransactionDTO>>>
    {
        private readonly ITransactionService _transactionService;

        public GetCustomerTransactionsQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<ResponseModel<List<TransactionDTO>>> Handle(GetCustomerTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.GetCustomerTransactions();
        }
    }
}
