using Awacash.Application.Role.Services;
using Awacash.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Role.Handler.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ResponseModel<bool>>
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<CreateRoleCommandHandler> _logger;
        public CreateRoleCommandHandler(IRoleService roleService, ILogger<CreateRoleCommandHandler> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        public async Task<ResponseModel<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.CreateRoleAsync(request.Name, request.Description, request.Permission);
        }
    }
}
