using AutoMapper;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.Customers.Handler.Queries.GetAllCustomers;
using Awacash.Application.Customers.Handler.Queries.GetPaginatedCustomers;
using Awacash.Application.Customers.Handler.Queries.GetSingleCustomer;
using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Handler.Queries.GetAllUsers;
using Awacash.Application.Users.Handler.Queries.GetPaginatedUsers;
using Awacash.Application.Users.Handler.Queries.GetUserById;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    //[Authorize]
    public class CustomersController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ResponseModel<CustomerDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<CustomerDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var getCustomerByIdQuery = new GetCustomerByIdQuery(id);
            var response = await _mediator.Send(getCustomerByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<CustomerDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllCustomerAsync()
        {
            var getAllCustomersQuery = new GetAllCustomersQuery();
            var response = await _mediator.Send(getAllCustomersQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<CustomerDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<CustomerDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPaginatedUserAsync([FromQuery] GetPaginatedCustomerQuery getPaginatedCustomerQuery)
        {
            var response = await _mediator.Send(getPaginatedCustomerQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
