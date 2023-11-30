using System;
using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Customers.Handler.Commands.UpdateCustomerNextOfKin
{
    public class UpdateCustomerNextOfKinCommand : IRequest<ResponseModel<bool>>
    {
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? NextOfKinName { get; set; }
        public string? NextOfKinRelationship { get; set; }
        public string? NextOfKinPhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? NextOfKinEmail { get; set; }
        public string? NextOfKinAddress { get; set; }
    }

    public class UpdateCustomerNextOfKinCommandValidator : AbstractValidator<UpdateCustomerNextOfKinCommand>
    {
        public UpdateCustomerNextOfKinCommandValidator()
        {
            //RuleFor(x => x.Address).NotEmpty().NotNull().WithMessage("Address is required");
            //RuleFor(x => x.State).NotEmpty().NotNull().WithMessage("State is required");
            //RuleFor(x => x.City).NotEmpty().NotNull().WithMessage("City is required");
            //RuleFor(x => x.NextOfKinName).NotEmpty().NotNull().WithMessage("Next of kin name is required");
            //RuleFor(x => x.NextOfKinRelationship).NotEmpty().NotNull().WithMessage("Next of kin relationship is required");
            //RuleFor(x => x.NextOfKinPhoneNumber).NotEmpty().NotNull().WithMessage("Next of kin phone is required");

            //RuleFor(x => x.Country).NotEmpty().NotNull().WithMessage("Next of kin country is required");
            //RuleFor(x => x.NextOfKinEmail).NotEmpty().NotNull().WithMessage("Next of kin email is required");
            //RuleFor(x => x.NextOfKinAddress).NotEmpty().NotNull().WithMessage("Next of kin address is required");

        }
    }
}

