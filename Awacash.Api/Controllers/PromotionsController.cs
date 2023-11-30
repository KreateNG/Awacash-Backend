using AutoMapper;
using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Handler.Queries.GetAllPromotion;
using Awacash.Application.Promotions.Handler.Queries.GetPromotionById;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{

    public class PromotionsController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public PromotionsController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
