using AutoMapper;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Handler.Commands.Register;
using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequestConfigurations.Handler.Queries.GetAllCardRequestConfiguration;
using Awacash.Application.CardRequestConfigurations.Handler.Queries.GetCardRequestConfigurationById;
using Awacash.Application.CardRequests.Handler.Commands.CreateCardRequest;
using Awacash.Contracts.Authentication;
using Awacash.Contracts.CardRequests;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Awacash.Api.Controllers
{

    [Authorize]
    public class CardRequestsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CardRequestsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [OpenApiOperation("CardRequest", "Customer request for card")]
        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CardRequest(CardRequestModel request)
        {
            var createCardRequestCommand = new CreateCardRequestCommand(request.AccountNumber, request.CardName, request.CardType, request.DeliveryAddress, request.CardConfigId); //_mapper.Map<RegisterCommand>(request); ;
            var response = await _mediator.Send(createCardRequestCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<List<CardRequestConfigurationDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<CardRequestConfigurationDTO>>), 400)]
        [HttpGet, Route("config")]
        public async Task<IActionResult> CardRequestConfiguration()
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
        [HttpGet, Route("config/{configId}")]
        public async Task<IActionResult> CardRequestConfigurationById(string configId)
        {
            var getCardRequestConfigurationByIdQuery = new GetCardRequestConfigurationByIdQuery(configId);
            var response = await _mediator.Send(getCardRequestConfigurationByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
