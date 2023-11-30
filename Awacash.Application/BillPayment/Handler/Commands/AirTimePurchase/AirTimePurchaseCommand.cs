using System;
using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.BillPayment.Handler.Commands.AirTimePurchase
{
    public record AirTimePurchaseCommand(string? AccountNumber, string? PaymentCode, string? CustomerMobile, string? Amount, string? Pin) : IRequest<ResponseModel<PaymentAdviceResponse>>;


    public class AirTimePurchaseCommandValidator : AbstractValidator<AirTimePurchaseCommand>
    {
        public AirTimePurchaseCommandValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().NotNull().WithMessage("Account number is required");
            RuleFor(x => x.Pin).NotEmpty().NotNull().WithMessage("Pin is required");
            RuleFor(x => x.PaymentCode).NotEmpty().NotNull().WithMessage("Payment code is required");
            RuleFor(x => x.Amount).NotEmpty().NotNull().WithMessage("Amount is required");
            RuleFor(x => x.CustomerMobile).NotEmpty().NotNull().WithMessage("Phone number is required");
        }
    }



    public record WalletAirTimePurchaseCommand(string? PaymentCode, string? CustomerMobile, string? Amount, string? Pin) : IRequest<ResponseModel<PaymentAdviceResponse>>;


    public class WalletAirTimePurchaseCommandValidator : AbstractValidator<WalletAirTimePurchaseCommand>
    {
        public WalletAirTimePurchaseCommandValidator()
        {

            RuleFor(x => x.Pin).NotEmpty().NotNull().WithMessage("Pin is required");
            RuleFor(x => x.PaymentCode).NotEmpty().NotNull().WithMessage("Payment code is required");
            RuleFor(x => x.Amount).NotEmpty().NotNull().WithMessage("Amount is required");
            RuleFor(x => x.CustomerMobile).NotEmpty().NotNull().WithMessage("Phone number is required");
        }
    }
}

