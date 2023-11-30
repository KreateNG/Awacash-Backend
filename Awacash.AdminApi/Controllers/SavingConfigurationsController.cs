using AutoMapper;
using Awacash.Application.Role.Handler.Commands.CreateRole;
using Awacash.Application.SavingsConfiguration.DTOs;
using Awacash.Application.SavingsConfiguration.Handler.Commands.CreateSavingConfiguration;
using Awacash.Contracts.Roles;
using Awacash.Contracts.SavingsConfiguration;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{

    public class SavingConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SavingConfigurationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(ResponseModel<SavingConfigurationDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SavingConfigurationDTO>), 400)]
        [HttpPost, Route("create-saving-configuration")]
        public async Task<IActionResult> CreateRole(CreateSavingConfigurationRequest request)
        {
            var createSavingConfiCommand = new CreateSavingConfigurationCommand(request.PlanName, request.PlanDescription, request.PlanDuration, request.PlanInterestRate, request.SavingTyp, request.ProductCode); //_mapper.Map<CreateSavingConfigurationCommand>(request);
            var response = await _mediator.Send(createSavingConfiCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
