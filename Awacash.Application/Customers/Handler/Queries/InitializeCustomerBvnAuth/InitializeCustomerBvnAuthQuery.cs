using Awacash.Application.Authentication.Handler.Queries.Login;
using Awacash.Shared;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Queries.InitializeCustomerBvnAuth
{
    public record InitializeCustomerBvnAuthQuery(string? Bvn) : IRequest<ResponseModel<string>>;
    public class InitializeCustomerBvnAuthQueryValidator : AbstractValidator<InitializeCustomerBvnAuthQuery>
    {
        public InitializeCustomerBvnAuthQueryValidator()
        {
            RuleFor(x => x.Bvn).NotEmpty().NotNull().WithMessage("Bvn is required");
        }
    }
}
