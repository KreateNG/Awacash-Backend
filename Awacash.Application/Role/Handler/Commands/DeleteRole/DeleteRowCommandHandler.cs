using System;
using Awacash.Application.Role.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Role.Handler.Commands.DeleteRole
{
    public class DeleteRowCommandHandler : IRequestHandler<DeleteRowCommand, ResponseModel<bool>>
    {
        private readonly IRoleService _roleService;
        public DeleteRowCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<ResponseModel<bool>> Handle(DeleteRowCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.DeleteRoleAsync(request.Id);
        }
    }
}

