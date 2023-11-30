using AutoMapper;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Handler.Commands.CreateSaving;
using Awacash.Application.Savings.Handler.Queries.GetAllSavingsById;
using Awacash.Application.Savings.Handler.Queries.GetSavingById;
using Awacash.Application.Users.Handler.Queries.GetUserById;
using Awacash.Contracts.Savings;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class SavingsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SavingsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 400)]
        [HttpPost, Route("create-saving")]
        public async Task<IActionResult> CreateSavingsAsync(CreateSavingRequest request)
        {
            var createSavingCommand = _mapper.Map<CreateSavingCommand>(request);
            var response = await _mediator.Send(createSavingCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllSavingsByIdAsync()
        {
            var GetAllSavingsByIdQuerry = new GetAllSavingsByIdQuery();
            var response = await _mediator.Send(GetAllSavingsByIdQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SavingDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetSavingByIdAsync(string id)
        {
            var GetSavingByIdQuerry = new GetSavingByIdQuery(id);
            var response = await _mediator.Send(GetSavingByIdQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
