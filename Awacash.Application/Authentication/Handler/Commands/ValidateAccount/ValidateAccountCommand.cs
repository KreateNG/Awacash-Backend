using System;
using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.ValidateAccount
{
    public class ValidateAccountCommand : IRequest<ResponseModel<string>>
    {
        public string? Code { get; set; }
        public string? Hash { get; set; }
    }
}

