using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.FilterModels;
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
    public class GetPaginatedRolesQuery : RoleFilterModel, IRequest<ResponseModel<PagedResult<RoleDTO>>>
    {

    }
}
