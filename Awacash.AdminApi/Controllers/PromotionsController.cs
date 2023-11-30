using AutoMapper;
using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Handler.Commands.AddPromotion;
using Awacash.Application.Promotions.Handler.Queries.GetAllPromotion;
using Awacash.Application.Promotions.Handler.Queries.GetPromotionById;
using Awacash.Application.Users.DTOs;
using Awacash.Application.Users.Handler.Commands.CreateUser;
using Awacash.Contracts.Promotions;
using Awacash.Contracts.Users;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.AdminApi.Controllers
{
    [Authorize]
    public class PromotionsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PromotionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ResponseModel<bool>), 200)]
        [ProducesResponseType(typeof(ResponseModel<bool>), 400)]
        [HttpPost, Route("")]
        public async Task<IActionResult> CreatePromotionAsync(AddPromotionRequest request)
        {
            var addPromotionCommand = _mapper.Map<AddPromotionCommand>(request);
            var response = await _mediator.Send(addPromotionCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<List<PromotionDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<PromotionDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAllPromotionAsync()
        {
            var getAllPromotionQuery = new GetAllPromotionQuery();
            var response = await _mediator.Send(getAllPromotionQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PromotionDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PromotionDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetPromotionByIdAsync(string id)
        {
            var getPromotionByIdQuery = new GetPromotionByIdQuery(id);
            var response = await _mediator.Send(getPromotionByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


    }
}
