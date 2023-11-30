using Awacash.Application.Authentication.Common;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Authentication.Handler.Queries.AdminLogin
{
    public record AdminLoginQuery(
        string Email,
        string Password) : IRequest<ResponseModel<AdminAuthResult>>;
}
