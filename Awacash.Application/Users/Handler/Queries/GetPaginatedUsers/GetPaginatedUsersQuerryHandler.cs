using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Services;
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
    public class GetPaginatedUsersQuerryHandler : IRequestHandler<GetPaginatedUsersQuerry, ResponseModel<PagedResult<UserDTO>>>
    {
        private readonly IUserService _userService;

        public GetPaginatedUsersQuerryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ResponseModel<PagedResult<UserDTO>>> Handle(GetPaginatedUsersQuerry request, CancellationToken cancellationToken)
        {
            return await _userService.GetPaginatedUserAsync(request);
        }
    }
}
