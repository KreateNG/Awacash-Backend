using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password,
        string DeviceId
        ): IRequest<ResponseModel<AuthenticationResult>>;
    
}
