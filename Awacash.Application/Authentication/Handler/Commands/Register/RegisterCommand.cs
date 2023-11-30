using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.Register
{
    public record RegisterCommand
    (
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string MiddleName,
        string PhoneNumber,
        string Pin, string Hash, string? ReferralCode) : IRequest<ResponseModel<AuthenticationResult>>;
}
