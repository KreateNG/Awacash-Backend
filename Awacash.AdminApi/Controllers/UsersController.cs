using AutoMapper;
using Awacash.Application.Role.Handler.Commands.CreateRole;
using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Handler.Commands.ChangeUserPassword;
using Awacash.Application.Users.Handler.Commands.CreateUser;
using Awacash.Application.Users.Handler.Commands.DeleteUser;
using Awacash.Application.Users.Handler.Commands.UpdateUser;
using Awacash.Application.Users.Handler.Queries.GetAllUsers;
using Awacash.Application.Users.Handler.Queries.GetPaginatedUsers;
using Awacash.Application.Users.Handler.Queries.GetUserById;
using Awacash.Contracts.Customers;
using Awacash.Contracts.Roles;
using Awacash.Contracts.Users;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    //[Authorize]
    public class UsersController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
        {
            var createRoleCommand = _mapper.Map<CreateUserCommand>(request);
            var response = await _mediator.Send(createRoleCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var getUserByIdQuery = new GetUserByIdQuery(id);
            var response = await _mediator.Send(getUserByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 400)]
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdatedUserByIdAsync(string id, UpdateUserRequest request)
        {
            var updateUserCommand = new UpdateUserCommand(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.UserName, request.RoleId);
            var response = await _mediator.Send(updateUserCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<UserDTO>), 400)]
        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(string id)
        {
            var deleteUserCommand = new DeleteUserCommand(id);
            var response = await _mediator.Send(deleteUserCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<UserDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<UserDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var getAllUsersQuery = new GetAllUsersQuery();
            var response = await _mediator.Send(getAllUsersQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<UserDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<UserDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPaginatedUserAsync([FromQuery] GetPaginatedUsersQuerry getPaginatedUsersQuerry)
        {
            var response = await _mediator.Send(getPaginatedUsersQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<string>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<string>>), 400)]
        [HttpPost, Route("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var changePasswordCommand = new ChangeUserPasswordCommand(request.OldPassword, request.NewPassword, request.ConfirmNewPassword);
            var response = await _mediator.Send(changePasswordCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
