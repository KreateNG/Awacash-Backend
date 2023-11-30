using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Role.Handler.Commands.DeleteRole
{
    public record DeleteRowCommand(string Id) : IRequest<ResponseModel<bool>>;
}

