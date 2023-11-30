using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, ResponseModel<TransactionDTO>>
    {
        private readonly ITransactionService _transactionService;

        public GetTransactionByIdQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<ResponseModel<TransactionDTO>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _transactionService.GetTransactionById(request.Id);
        }
    }
}
