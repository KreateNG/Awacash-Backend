using AutoMapper;
using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.FilterModels;
using Awacash.Application.Role.Handler.Commands.CreateRole;
using Awacash.Application.Role.Handler.Commands.DeleteRole;
using Awacash.Application.Role.Handler.Commands.UpdateRole;
using Awacash.Application.Role.Handler.Queries.GetAllRoles;
using Awacash.Application.Role.Handler.Queries.GetPaginatedRoles;
using Awacash.Application.Role.Handler.Queries.GetPermissions;
using Awacash.Application.Role.Handler.Queries.GetSingleRole;
using Awacash.Contracts.Roles;
using Awacash.Domain.Common.Models;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    //[Authorize]
    public class RolesController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RolesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateRole(CreateRoleRequest request)
        {
            var createRoleCommand = _mapper.Map<CreateRoleCommand>(request);
            var response = await _mediator.Send(createRoleCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdateRole(UpdateRoleRequest request, string id)
        {
            var updateRoleCommand = new UpdateRoleCommand(id, request.Name, request.Description, request.Permission);
            var response = await _mediator.Send(updateRoleCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<RoleDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<RoleDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var getRoleByIdQuery = new GetRoleByIdQuery(id);
            var response = await _mediator.Send(getRoleByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<RoleDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<RoleDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllRole()
        {
            var getAllRoleQuery = new GetAllRolesQuery();
            var response = await _mediator.Send(getAllRoleQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<RoleDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<RoleDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetAllRolePaged([FromQuery] GetPaginatedRolesQuery request)
        {
            var response = await _mediator.Send(request);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var deleteRowCommand = new DeleteRowCommand(id);
            var response = await _mediator.Send(deleteRowCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<PermissionDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<PermissionDto>>), 400)]
        [HttpGet, Route("permissions")]
        public async Task<IActionResult> GetAllRolePermission()
        {
            var getPermissionsQuery = new GetPermissionsQuery();
            var response = await _mediator.Send(getPermissionsQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
