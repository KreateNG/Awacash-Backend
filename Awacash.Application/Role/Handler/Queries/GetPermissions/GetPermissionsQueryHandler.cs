using System;
using Awacash.Application.Role.Services;
using Awacash.Domain.Common.Models;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Role.Handler.Queries.GetPermissions
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, ResponseModel<List<PermissionDto>>>
    {
        private readonly IRoleService _roleService;

        public GetPermissionsQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<ResponseModel<List<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetAllPermissionsAsync();
        }
    }
}

