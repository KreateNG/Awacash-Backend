using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.DashBoard.DTOs;
using Awacash.Application.DashBoard.Handler.Queries;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class DashBoardController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DashBoardController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(ResponseModel<DashBoardDto>), 200)]
        [ProducesResponseType(typeof(ResponseModel<DashBoardDto>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetDashBoardAsync()
        {
            var getDashBoardDataQuery = new GetDashBoardDataQuery();
            var response = await _mediator.Send(getDashBoardDataQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}

