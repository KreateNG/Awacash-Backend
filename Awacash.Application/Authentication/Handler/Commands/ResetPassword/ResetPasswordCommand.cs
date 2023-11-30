using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string ConfirmPasswprd, string Password, string Hash) : IRequest<ResponseModel<bool>>;
}
