using AutoMapper;
using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequestConfigurations.Handler.Commands.CreateCardRequestConfiguration;
using Awacash.Application.CardRequestConfigurations.Handler.Queries.GetAllCardRequestConfiguration;
using Awacash.Application.CardRequestConfigurations.Handler.Queries.GetCardRequestConfigurationById;
using Awacash.Application.Role.Handler.Commands.CreateRole;
using Awacash.Contracts.CardRequestConfigurations;
using Awacash.Contracts.Roles;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class CardRequestConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CardRequestConfigurationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateCardRequestConfiguration(CardRequestConfigurationModel request)
        {
            var createCardRequestConfigurationCommand = new CreateCardRequestConfigurationCommand(request.IssuerName, request.Price, request.CardType);
            var response = await _mediator.Send(createCardRequestConfigurationCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<CardRequestConfigurationDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<CardRequestConfigurationDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetCardRequestConfiguration()
        {
            var getAllCardRequestConfigurationQuery = new GetAllCardRequestConfigurationQuery();
            var response = await _mediator.Send(getAllCardRequestConfigurationQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<CardRequestConfigurationDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<CardRequestConfigurationDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetCardRequestConfigurationById(string id)
        {
            var GetCardRequestConfigurationByIdQuery = new GetCardRequestConfigurationByIdQuery(id);
            var response = await _mediator.Send(GetCardRequestConfigurationByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
