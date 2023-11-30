using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Domain.Enums;
using Awacash.Shared;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.CardRequests.Handler.Commands.CreateCardRequest
{
    public record CreateCardRequestCommand(string? AccountNumber, string? CardName, CardType? CardType, string? DeliveryAddress, string? CardConfigId) : IRequest<ResponseModel<bool>>;

    public class CreateCardRequestCommandValidator : AbstractValidator<CreateCardRequestCommand>
    {
        public CreateCardRequestCommandValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().NotNull().WithMessage("Account number is required");
            RuleFor(x => x.CardName).NotEmpty().NotNull().WithMessage("Card name is required");
            RuleFor(x => x.CardType).NotEmpty().NotNull().WithMessage("Card type is required");
            RuleFor(x => x.DeliveryAddress).NotEmpty().NotNull().WithMessage("Delivery address is required");
            RuleFor(x => x.CardConfigId).NotEmpty().NotNull().WithMessage("Card config id is required");
        }
    }
}
