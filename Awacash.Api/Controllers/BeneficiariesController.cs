using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Application.Beneficiaries.Handler.Queries.GetAllCustomerBeneficary;
using Awacash.Application.Beneficiaries.Handler.Queries.GetBeneficaryById;
using Awacash.Application.Promotions.DTOs;
using Awacash.Application.Promotions.Handler.Queries.GetAllPromotion;
using Awacash.Application.Promotions.Handler.Queries.GetPromotionById;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class BeneficiariesController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public BeneficiariesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<BeneficiaryDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<BeneficiaryDTO>), 400)]
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetBeneficiaryByIdAsync(string id)
        {
            var getBeneficaryByIdQuery = new GetBeneficaryByIdQuery(id);
            var response = await _mediator.Send(getBeneficaryByIdQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<BeneficiaryDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<BeneficiaryDTO>>), 400)]
        [HttpGet, Route("")]
        public async Task<IActionResult> GetAlltBeneficiariesAsync()
        {
            var getAllCustomerBeneficaryQuery = new GetAllCustomerBeneficaryQuery();
            var response = await _mediator.Send(getAllCustomerBeneficaryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
