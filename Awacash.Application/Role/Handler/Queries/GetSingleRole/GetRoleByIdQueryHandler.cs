using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Queries.GetSingleRole
{    
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ResponseModel<RoleDTO>>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<ResponseModel<RoleDTO>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetRoleByIdAsync(request.Id);
        }
    }
}
