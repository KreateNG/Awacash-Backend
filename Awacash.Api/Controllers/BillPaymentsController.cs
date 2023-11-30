using AutoMapper;
using Awacash.Application.BillPayment.Handler.Commands.AirTimePurchase;
using Awacash.Application.BillPayment.Handler.Commands.SendPaymentAdvice;
using Awacash.Application.BillPayment.Handler.Commands.ValidateCustomer;
using Awacash.Application.BillPayment.Handler.Queries.GetBiller;
using Awacash.Application.BillPayment.Handler.Queries.GetBillerCategory;
using Awacash.Application.BillPayment.Handler.Queries.GetBillerPaymentItem;
using Awacash.Contracts.BillPayment;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class BillPaymentsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BillPaymentsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ResponseModel<List<BillerCategory>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<BillerCategory>>), 400)]
        [HttpGet, Route("get-billers")]
        public async Task<IActionResult> GetBillers()
        {
            var getBillerQuery = new GetBillerQuery();
            var response = await _mediator.Send(getBillerQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 400)]
        [HttpGet, Route("get-billers-category/{categoryId}")]
        public async Task<IActionResult> GetBillerCategory(int categoryId)
        {
            var getBillerCategoryQuery = new GetBillerCategoryQuery(categoryId);
            var response = await _mediator.Send(getBillerCategoryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 400)]
        [HttpGet, Route("get-airtime-billers")]
        public async Task<IActionResult> GetAirTimeBillers()
        {
            var getBillerCategoryQuery = new GetBillerCategoryQuery(3);
            var response = await _mediator.Send(getBillerCategoryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 400)]
        [HttpGet, Route("get-data-billers")]
        public async Task<IActionResult> GetDataBillers()
        {
            var getBillerCategoryQuery = new GetBillerCategoryQuery(4);
            var response = await _mediator.Send(getBillerCategoryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Biller>>), 400)]
        [HttpGet, Route("get-internet-billers")]
        public async Task<IActionResult> GetTnternetBillers()
        {
            var getBillerCategoryQuery = new GetBillerCategoryQuery(5);
            var response = await _mediator.Send(getBillerCategoryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 400)]
        [HttpGet, Route("get-payment-items/{billerId}")]
        public async Task<IActionResult> GetPaymentItem(string billerId)
        {
            var getBillerPaymentItemQuery = new GetBillerPaymentItemQuery(billerId);
            var response = await _mediator.Send(getBillerPaymentItemQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 400)]
        [HttpPost, Route("validate-customer")]
        public async Task<IActionResult> ValidateCustomer(ValidateCustomerRequest request)
        {
            var validateCustomerCommand = new ValidateCustomerCommand(request.CustomerId, request.PaymentCode);
            var response = await _mediator.Send(validateCustomerCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<Paymentitem>>), 400)]
        [HttpPost, Route("send-payment-advice")]
        public async Task<IActionResult> SendPaymentAdvice(PaymentAdviceRequest request)
        {
            var sendPaymentAdviceCommand = new SendPaymentAdviceCommand(request.AccountNumber, request.PaymentCode, request.CustomerId, request.CustomerMobile, request.CustomerEmail, request.Amount, request.Pin);
            var response = await _mediator.Send(sendPaymentAdviceCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 400)]
        [HttpPost, Route("bill-payment")]
        public async Task<IActionResult> BillPayment(PaymentAdviceRequest request)
        {
            var sendPaymentAdviceCommand = new SendPaymentAdviceCommand(request.AccountNumber, request.PaymentCode, request.CustomerId, request.CustomerMobile, request.CustomerEmail, request.Amount, request.Pin);
            var response = await _mediator.Send(sendPaymentAdviceCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 400)]
        [HttpPost, Route("air-time")]
        public async Task<IActionResult> AirTimePurchase(AirTimePurchaseRequest request)
        {
            var airTimePurchaseCommand = _mapper.Map<AirTimePurchaseCommand>(request);
            var response = await _mediator.Send(airTimePurchaseCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }



        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 400)]
        [HttpPost, Route("wallet/bill-payment")]
        public async Task<IActionResult> WalletBillPayment(WalletPaymentAdviceRequest request)
        {
            var sendPaymentAdviceCommand = new SendWalletPaymentAdviceCommand(request.PaymentCode, request.CustomerId, request.CustomerMobile, request.CustomerEmail, request.Amount, request.Pin);
            var response = await _mediator.Send(sendPaymentAdviceCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<PaymentAdviceResponse>), 400)]
        [HttpPost, Route("wallet/air-time")]
        public async Task<IActionResult> WalletAirTimePurchase(WalletAirTimePurchaseRequest request)
        {
            var airTimePurchaseCommand = new WalletAirTimePurchaseCommand(request.PaymentCode, request.CustomerMobile, request.Amount, request.Pin);
            var response = await _mediator.Send(airTimePurchaseCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
