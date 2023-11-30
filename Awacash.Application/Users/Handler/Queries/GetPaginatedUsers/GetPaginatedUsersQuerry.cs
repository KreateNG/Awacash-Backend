using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.FilterModels;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Users.Handler.Queries.GetPaginatedUsers
{
    public class GetPaginatedUsersQuerry: UserFilterModel, IRequest<ResponseModel<PagedResult<UserDTO>>>
    {
    }
}
