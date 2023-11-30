using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Commands.ChangePassword
{
    public record ChangePasswordCommand(string OldPassword, string newPassword):IRequest<ResponseModel<bool>>;
}
