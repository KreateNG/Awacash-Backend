using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.PoneVerification
{
    public record SendPhoneVerificationCodeCommand(string PhoneNumber):IRequest<ResponseModel<string>>;
}
