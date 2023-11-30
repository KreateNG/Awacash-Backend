using AutoMapper;
using Awacash.Application.CardRequests.Handler.Commands.CreateCardRequest;
using Awacash.Application.DisputeLogs.Handler.Commands.CreateDispuste;
using Awacash.Contracts.CardRequests;
using Awacash.Contracts.DisputeLogs;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Awacash.Api.Controllers
{
    
    [Authorize]
    public class DisputeLogsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public DisputeLogsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [OpenApiOperation("Create dispute", "Customer log dispute")]
        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateDisputeAsync(CreateDipusteLogRequest request)
        {
            var createDisputeCommand = _mapper.Map<CreateDisputeCommand>(request); ;
            var response = await _mediator.Send(createDisputeCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
