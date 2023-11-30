using System;
using Awacash.Application.Transactions.Services;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transactions.Handlers.Queries.TransactionStatus
{
    public class TransactionStatusQueryHandler : IRequestHandler<TransactionStatusQuery, ResponseModel<List<TransactionQuery>>>
    {
        private readonly ITransactionService _transactionService;

        public TransactionStatusQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<ResponseModel<List<TransactionQuery>>> Handle(TransactionStatusQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.TransQuery(request.CrcAccount);
        }
    }
}

