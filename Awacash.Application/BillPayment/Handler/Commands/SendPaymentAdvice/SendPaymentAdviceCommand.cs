using Awacash.Application.BillPayment.Handler.Commands.AirTimePurchase;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Commands.SendPaymentAdvice
{
    public record SendPaymentAdviceCommand(string? AccountNumber, string? PaymentCode, string? CustomerId, string? CustomerMobile, string? CustomerEmail, string? Amount, string? Pin) : IRequest<ResponseModel<PaymentAdviceResponse>>;

    public class SendPaymentAdviceCommandValidator : AbstractValidator<SendPaymentAdviceCommand>
    {
        public SendPaymentAdviceCommandValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().NotNull().WithMessage("Account number is required");
            RuleFor(x => x.Pin).NotEmpty().NotNull().WithMessage("Pin is required");
            RuleFor(x => x.PaymentCode).NotEmpty().NotNull().WithMessage("Payment code is required");
            RuleFor(x => x.Amount).NotEmpty().NotNull().WithMessage("Amount is required");
            RuleFor(x => x.CustomerId).NotEmpty().NotNull().WithMessage("customer ID is required");
        }
    }


    public record SendWalletPaymentAdviceCommand(string? PaymentCode, string? CustomerId, string? CustomerMobile, string? CustomerEmail, string? Amount, string? Pin) : IRequest<ResponseModel<PaymentAdviceResponse>>;

    public class SendWalletPaymentAdviceCommandValidator : AbstractValidator<SendWalletPaymentAdviceCommand>
    {
        public SendWalletPaymentAdviceCommandValidator()
        {
            RuleFor(x => x.Pin).NotEmpty().NotNull().WithMessage("Pin is required");
            RuleFor(x => x.PaymentCode).NotEmpty().NotNull().WithMessage("Payment code is required");
            RuleFor(x => x.Amount).NotEmpty().NotNull().WithMessage("Amount is required");
            RuleFor(x => x.CustomerId).NotEmpty().NotNull().WithMessage("customer ID is required");
        }
    }
}
