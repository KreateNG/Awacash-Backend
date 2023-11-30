using System;
using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.SendAccountVerificationCode
{
    public class SendAccountVerificationCodeCommand : IRequest<ResponseModel<AccountValidationResponse>>
    {
        public string? AccountNumber { get; set; }
    }
}

