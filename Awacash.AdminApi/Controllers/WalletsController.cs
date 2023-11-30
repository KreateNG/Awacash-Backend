using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Awacash.Application.Wallets.DTOs;
using Awacash.Application.Wallets.Handler.Commands.UpdateWalletStatus;
using Awacash.Application.Wallets.Handler.Queries.GetAllWallets;
using Awacash.Application.Wallets.Handler.Queries.GetPaginatedWallet;
using Awacash.Application.Wallets.Handler.Queries.GetWalletById;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awacash.AdminApi.Controllers
{

    public class WalletsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WalletsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ResponseModel<WalletDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<WalletDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetWalletByIdAsync(string id)
        {
            var getWalletByIdQuery = new GetWalletByIdQuery(id);
            var response = await _mediator.Send(getWalletByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPut, Route("{id}/status")]
        public async Task<IActionResult> UpdatedWalletStatusByIdAsync(string id)
        {
            var updateWalletStatusCommand = new UpdateWalletStatusCommand(id);
            var response = await _mediator.Send(updateWalletStatusCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<List<WalletDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<WalletDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllWalletAsync()
        {
            var getAllWalletsQuery = new GetAllWalletsQuery();
            var response = await _mediator.Send(getAllWalletsQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PagedResult<WalletDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PagedResult<WalletDTO>>), 400)]
        [HttpGet, Route("paged")]
        public async Task<IActionResult> GetPaginatedWalletAsync([FromQuery] GetPaginatedWalletQuery getPaginatedWalletsQuerry)
        {
            var response = await _mediator.Send(getPaginatedWalletsQuerry);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}

