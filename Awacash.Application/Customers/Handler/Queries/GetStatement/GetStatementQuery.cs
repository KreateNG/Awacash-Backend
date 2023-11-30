using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Customers.Handler.Queries.GetStatement;

public record GetStatementQuery(string? AccountNumber, DateTime? From, DateTime? To) : IRequest<ResponseModel>;

public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
{
    public GetStatementQueryValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().NotNull().WithMessage("Account number is required");
        RuleFor(x => x.To).NotEmpty().NotNull().WithMessage("End date is required");
        RuleFor(x => x.From).NotEmpty().NotNull().WithMessage("Start date is required");
    }
}