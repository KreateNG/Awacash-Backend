using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.ChangePin
{
    public record ChangePinCommand(string OldPin, string NewPin, string ConfirmNewPin) : IRequest<ResponseModel<bool>>;
}
