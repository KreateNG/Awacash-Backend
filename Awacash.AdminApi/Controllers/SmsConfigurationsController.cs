using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.SmsTemplateConfigurations.DTOs;
using Awacash.Application.SmsTemplateConfigurations.Handler.Commands.CreateSmsTemplate;
using Awacash.Application.SmsTemplateConfigurations.Handler.Commands.UpdateSmsTemplate;
using Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetAllSmsTemplate;
using Awacash.Application.SmsTemplateConfigurations.Handler.Queries.GetSmsTemplateById;
using Awacash.Contracts.SmsConfigurations;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class SmsConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public SmsConfigurationsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<SmsTemplateDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SmsTemplateDto>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateSmsAsync([FromBody] CreateSmsConfigurationRequest request)
        {
            var createSmsTemplateCommand = _mapper.Map<CreateSmsTemplateCommand>(request);
            var response = await _mediator.Send(createSmsTemplateCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdateSmsAsync(string id, [FromBody] UpdateSmsConfigurationRequest request)
        {
            var createSmsTemplateCommand = new UpdateSmsTemplateCommand(request.Message, id);
            var response = await _mediator.Send(createSmsTemplateCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<SmsTemplateDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<SmsTemplateDto>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetSmsByIdAsync(string id)
        {
            var getSmsTemplateByIdQuery = new GetSmsTemplateByIdQuery(id);
            var response = await _mediator.Send(getSmsTemplateByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<SmsTemplateDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<SmsTemplateDto>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllSmsAsync()
        {
            var getSmsAllTemplateQuery = new GetSmsAllTemplateQuery();
            var response = await _mediator.Send(getSmsAllTemplateQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}

