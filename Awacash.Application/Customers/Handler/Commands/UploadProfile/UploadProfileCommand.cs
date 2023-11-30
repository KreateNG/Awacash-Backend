using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.UploadProfile
{
    public record UploadProfileCommand(string PasswordBase64) :IRequest<ResponseModel<string>>;
}
