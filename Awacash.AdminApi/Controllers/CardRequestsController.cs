using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.CardRequestConfigurations.DTOs;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.FilterModels;
using Awacash.Application.CardRequests.Handler.Queries.GetAll;
using Awacash.Application.CardRequests.Handler.Queries.GetById;
using Awacash.Application.CardRequests.Handler.Queries.GetPagedList;
using Awacash.Domain.Entities;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
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
        [ProducesResponseType(typeof(ResponseModel<List<CardRequestDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<CardRequestDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllCardRequest()
        {
            var getAllCardRequestQuery = new GetAllCardRequestQuery();
            var response = await _mediator.Send(getAllCardRequestQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [ProducesResponseType(typeof(ResponseModel<PagedResult<CardRequestDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<CardRequestDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPagedCardRequest([FromQuery] GetPaginatedCardRequestQuery getPaginatedCardRequestQuery)
        {
            var response = await _mediator.Send(getPaginatedCardRequestQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [ProducesResponseType(typeof(ResponseModel<CardRequestDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<CardRequestDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetCardRequestById(string id)
        {
            var getCardRequestByIdQuery = new GetCardRequestByIdQuery(id);
            var response = await _mediator.Send(getCardRequestByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}

