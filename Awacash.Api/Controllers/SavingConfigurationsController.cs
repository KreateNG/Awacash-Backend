using AutoMapper;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Savings.Handler.Queries.GetAllSavingsById;
using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Handler.Queries.GetAllSavingConfiguration;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    
    [Authorize]
    public class SavingConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SavingConfigurationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<SavingConfigurationDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<SavingConfigurationDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllSavingConfigAsync()
        {
            var getAllSavingConfigurationQuery = new GetAllSavingConfigurationQuery();
            var response = await _mediator.Send(getAllSavingConfigurationQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
