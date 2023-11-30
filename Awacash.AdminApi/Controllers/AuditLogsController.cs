using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.AuditLogs.DTOs;
using Awacash.Application.AuditLogs.Handler.Queries.GetAuditLogById;
using Awacash.Application.AuditLogs.Handler.Queries.GetPagedAuditLog;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Handler.Queries.GetPaginatedCustomers;
using Awacash.Application.Customers.Handler.Queries.GetSingleCustomer;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class AuditLogsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuditLogsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(ResponseModel<PagedResult<AuditLogDto>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<AuditLogDto>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPaginatedAuditLogAsync([FromQuery] GetPagedAuditLogQuery getPagedAuditLogQuery)
        {
            var response = await _mediator.Send(getPagedAuditLogQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<AuditLogDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<AuditLogDto>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetAuditLogByIdAsync(string id)
        {
            var getAuditLogByIdQuery = new GetAuditLogByIdQuery(id);
            var response = await _mediator.Send(getAuditLogByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}

