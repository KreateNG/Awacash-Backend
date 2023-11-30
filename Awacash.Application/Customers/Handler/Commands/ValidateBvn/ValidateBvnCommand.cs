using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Customers.Handler.Commands.ValidateBvn
{
    public record ValidateBvnCommand(string FirstName, string LastName, DateTime DateOfBirth, string AccessToken) :IRequest<ResponseModel<bool>>;
}
