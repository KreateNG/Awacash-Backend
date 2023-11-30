using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.SetPin
{
    public record SetPinCommand(string Pin):IRequest<ResponseModel<bool>>;
}
