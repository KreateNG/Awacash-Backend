using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.AdminApi.Controllers;
using Awacash.Application.Customers.DTOs;
using Awacash.Application.FeeConfigurations.DTOs;
using Awacash.Application.FeeConfigurations.Handler.Commands.CreateFeeConfiguration;
using Awacash.Application.FeeConfigurations.Handler.Queries.GetAllFee;
using Awacash.Application.FeeConfigurations.Handler.Queries.GetFeeById;
using Awacash.Contracts.FeeConfigurations;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class FeeConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FeeConfigurationsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateFeeConfigAsync(CreateFeeConfigurationRequest request)
        {
            var createFeeConfigurationCommand = _mapper.Map<CreateFeeConfigurationCommand>(request);
            var response = await _mediator.Send(createFeeConfigurationCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<FeeConfigurationDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<FeeConfigurationDto>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllFeesConfigAsync()
        {
            var getAllFeesQuery = new GetAllFeesQuery();
            var response = await _mediator.Send(getAllFeesQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<FeeConfigurationDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<FeeConfigurationDto>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetFeeConfigByIdAsync(string id)
        {
            var getFeeByIdQuery = new GetFeeByIdQuery(id);
            var response = await _mediator.Send(getFeeByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}

