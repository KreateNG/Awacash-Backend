using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.EmailTemplateConfigurations.Handler.Commands.UpdateEmailTemplate;
using Awacash.Application.EmailTemplateConfigurations.DTOs;
using Awacash.Application.EmailTemplateConfigurations.Handler.Commands.CreateEmailTemplate;
using Awacash.Contracts.EmailConfigurations;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetAllEmailTemplate;
using Awacash.Application.EmailTemplateConfigurations.Handler.Queries.GetEmailTemplateById;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class EmailConfigurationsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EmailConfigurationsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<EmailTemplateDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<EmailTemplateDto>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateEmailAsync([FromBody] CreateEmailConfigurationRequest request)
        {
            var createEmailTemplateCommand = _mapper.Map<CreateEmailTemplateCommand>(request);
            var response = await _mediator.Send(createEmailTemplateCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPut, Route("{Id}")]
        public async Task<IActionResult> UpdateEmailAsync(string Id, [FromBody] UpdateEmailConfigurationRequest request)
        {
            var updateEmailTemplateCommand = new UpdateEmailTemplateCommand(Id, request.Body, request.Subject);
            var response = await _mediator.Send(updateEmailTemplateCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<EmailTemplateDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<EmailTemplateDto>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllEmailAsync()
        {
            var getEmailAllTemplateQuery = new GetEmailAllTemplateQuery();
            var response = await _mediator.Send(getEmailAllTemplateQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<EmailTemplateDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<EmailTemplateDto>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetEmailByIdAsync(string id)
        {
            var getEmailTemplateByIdQuery = new GetEmailTemplateByIdQuery(id);
            var response = await _mediator.Send(getEmailTemplateByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}

