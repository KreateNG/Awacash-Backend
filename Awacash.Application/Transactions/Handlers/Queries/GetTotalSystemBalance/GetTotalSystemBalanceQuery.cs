using System;
using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transactions.Handlers.Queries.GetTotalSystemBalance
{
    public record GetTotalSystemBalanceQuery() : IRequest<ResponseModel<WalletBalanceDto>>;

    public class GetTotalSystemBalanceQueryHandler : IRequestHandler<GetTotalSystemBalanceQuery, ResponseModel<WalletBalanceDto>>
    {
        private readonly ITransactionService _transactionService;
        public GetTotalSystemBalanceQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<ResponseModel<WalletBalanceDto>> Handle(GetTotalSystemBalanceQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.GetWalletBalance();
        }
    }
}

