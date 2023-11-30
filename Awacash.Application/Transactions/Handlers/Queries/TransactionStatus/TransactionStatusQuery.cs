using System;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Transactions.Handlers.Queries.TransactionStatus
{
    public record TransactionStatusQuery(string CrcAccount) : IRequest<ResponseModel<List<TransactionQuery>>>;
}

