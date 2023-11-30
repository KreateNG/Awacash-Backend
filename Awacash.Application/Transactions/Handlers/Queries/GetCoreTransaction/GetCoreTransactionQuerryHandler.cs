using System;
using Awacash.Application.Transactions.Services;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transactions.Handlers.Queries.GetCoreTransaction
{
    public class GetCoreTransactionQuerryHandler : IRequestHandler<GetCoreTransactionQuerry, ResponseModel<List<TransactionResponseDto>>>
    {
        private readonly ITransactionService _transactionService;
        public GetCoreTransactionQuerryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<ResponseModel<List<TransactionResponseDto>>> Handle(GetCoreTransactionQuerry request, CancellationToken cancellationToken)
        {
            if (!request.StartDate.HasValue)
            {
                request.StartDate = DateTime.UtcNow.AddMonths(-1);
            }
            if (!request.EndDate.HasValue)
            {
                request.EndDate = DateTime.UtcNow;
            }
            return await _transactionService.GetCoreTransactions(request.AccountNumber, request.StartDate.Value, request.EndDate.Value);
        }
    }
}

