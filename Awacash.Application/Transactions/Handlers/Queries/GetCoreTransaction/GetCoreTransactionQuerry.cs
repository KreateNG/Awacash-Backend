using System;
using Awacash.Application.Savings.Handler.Commands.CreateSaving;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Transactions.Handlers.Queries.GetCoreTransaction;

public class GetCoreTransactionQuerry : IRequest<ResponseModel<List<TransactionResponseDto>>>
{
    public string? AccountNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
public class GetCoreTransactionQuerryValidator : AbstractValidator<GetCoreTransactionQuerry>
{
    public GetCoreTransactionQuerryValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required");
        //RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required");
        //RuleFor(x => x.EndDate).NotEmpty().WithMessage("End date is required");
    }
}

