using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.Services;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Queries.GetPaginatedRoles
{
    public class GetPaginatedRolesQueryHandler : IRequestHandler<GetPaginatedRolesQuery, ResponseModel<PagedResult<RoleDTO>>>
    {
        private readonly IRoleService _roleService;

        public GetPaginatedRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<ResponseModel<PagedResult<RoleDTO>>> Handle(GetPaginatedRolesQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetPaginatedRoleAsync(request);
        }
    }
}
