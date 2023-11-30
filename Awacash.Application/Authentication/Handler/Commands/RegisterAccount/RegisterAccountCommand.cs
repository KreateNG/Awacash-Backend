using System;
using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.RegisterAccount
{
    public class RegisterAccountCommand : IRequest<ResponseModel<AuthenticationResult>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? Pin { get; set; }
        public string? Hash { get; set; }

    }
}

