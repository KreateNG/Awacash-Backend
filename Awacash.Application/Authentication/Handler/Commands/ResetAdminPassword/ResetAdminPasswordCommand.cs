using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.ResetAdminPassword
{
    public record ResetAdminPasswordCommand(string Email, string ConfirmPasswprd, string Password) : IRequest<ResponseModel<bool>>;

}

