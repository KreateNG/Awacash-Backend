using AutoMapper;
using Awacash.Application.FeeConfigurations.Handler.Queries.GetFee;
using Awacash.Application.Transfers.Handlers.Commands.InterBankTransfer;
using Awacash.Application.Transfers.Handlers.Commands.InterBankWalletTransfer;
using Awacash.Application.Transfers.Handlers.Commands.LocalTransfer;
using Awacash.Application.Transfers.Handlers.Commands.LocalWalletTransfer;
using Awacash.Application.Transfers.Handlers.Commands.NipNameEnquiry;
using Awacash.Application.Transfers.Handlers.Commands.NipWalletNameEnquiry;
using Awacash.Application.Transfers.Handlers.Commands.OwnAccountTransfer;
using Awacash.Application.Transfers.Handlers.Queries.BalanceEnquiry;
using Awacash.Application.Transfers.Handlers.Queries.Banks;
using Awacash.Application.Transfers.Handlers.Queries.LocalTransferNameEnquiry;
using Awacash.Application.Transfers.Handlers.Queries.LocalWalletTransferNameEnquiry;
using Awacash.Application.Transfers.Handlers.Queries.WalletBalanceEnquiry;
using Awacash.Application.Transfers.Handlers.Queries.WalletBanks;
using Awacash.Application.Transfers.Services;
using Awacash.Contracts.Transactions;
using Awacash.Domain.Enums;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awacash.Api.Controllers
{
    [Authorize]
    public class TransfersController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITransferService _transferService;
        public TransfersController(IMapper mapper, IMediator mediator, ITransferService transferService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _transferService = transferService;
        }

        #region commands
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("local-transfer")]
        public async Task<IActionResult> PostLocalTransferAsync(LocalTransferRequest request)
        {
            var localTransferCommand = new LocalTransferCommand(request.DebitAccount, request.CreditAccount, request.Amount, request.TransactionPin, request.Narration, request.TransactionReference, request.AddAsBeneficary); //_mapper.Map<LocalTransferCommand>(request);
            var response = await _mediator.Send(localTransferCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("own-account-transfer")]
        public async Task<IActionResult> PostOwnAccountTransferAsync(LocalTransferRequest request)
        {
            var ownAccountTransferCommand = new OwnAccountTransferCommand(request.DebitAccount, request.CreditAccount, request.Amount, request.TransactionPin, request.Narration, request.TransactionReference, request.AddAsBeneficary); //_mapper.Map<LocalTransferCommand>(request);
            var response = await _mediator.Send(ownAccountTransferCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 400)]
        [HttpPost, Route("inter-bank-transfer")]
        public async Task<IActionResult> PostInterBankTransferAsync(InterBankTransferRequest request)
        {
            var interBankTransferCommand = new InterBankTransferCommand(request.DebitAccount, request.BankCode, request.CreditAccount, request.Amount, request.TransactionPin, request.Narration, request.TransactionReference, request.AddAsBeneficary); //_mapper.Map<InterBankTransferCommand>(request);
            var response = await _mediator.Send(interBankTransferCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 400)]
        [HttpPost, Route("inter-bank-name-enquiry")]
        public async Task<IActionResult> PostNipNameEnquiryAsync(NipNameEnquiryRequest request)
        {
            var nipNameEnquiryCommand = new NipNameEnquiryCommand(request.BankCode, request.AccountNumber); //_mapper.Map<NipNameEnquiryCommand>(request);
            var response = await _mediator.Send(nipNameEnquiryCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        #endregion


        #region wallet commands
        [ProducesResponseType(typeof(ResponseModel), 200)]
        [ProducesResponseType(typeof(ResponseModel), 400)]
        [HttpPost, Route("wallet/local-transfer")]
        public async Task<IActionResult> PostwalletLocalTransferAsync(LocalTransferRequest request)
        {
            var localTransferCommand = new LocalWalletTransferCommand(request.DebitAccount, request.CreditAccount, request.Amount, request.TransactionPin, request.Narration, request.TransactionReference, request.AddAsBeneficary); //_mapper.Map<LocalTransferCommand>(request);
            var response = await _mediator.Send(localTransferCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipTransferResponse>), 400)]
        [HttpPost, Route("wallet/inter-bank-transfer")]
        public async Task<IActionResult> PostWalletInterBankTransferAsync(InterBankTransferRequest request)
        {
            var interBankTransferCommand = new InterBankWalletTransferCommand(request.DebitAccount, request.BankCode, request.CreditAccount, request.Amount, request.TransactionPin, request.Narration, request.TransactionReference, request.AddAsBeneficary); //_mapper.Map<InterBankTransferCommand>(request);
            var response = await _mediator.Send(interBankTransferCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 400)]
        [HttpPost, Route("wallet/inter-bank-name-enquiry")]
        public async Task<IActionResult> PostWalletNipNameEnquiryAsync(NipNameEnquiryRequest request)
        {
            var nipNameEnquiryCommand = new NipWalletNameEnquiryCommand(request.BankCode, request.AccountNumber); //_mapper.Map<NipNameEnquiryCommand>(request);
            var response = await _mediator.Send(nipNameEnquiryCommand);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        #endregion 

        #region Queries
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 400)]
        [HttpGet, Route("local-transfer-name-enquiry/{phoneNumber}")]
        public async Task<IActionResult> LocalTransferNameEnquiryAsync(string phoneNumber)
        {
            var localTransferNameEnquiryQuery = new LocalTransferNameEnquiryQuery(phoneNumber);
            var response = await _mediator.Send(localTransferNameEnquiryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 400)]
        [HttpGet, Route("get-banks")]
        public async Task<IActionResult> GetBanksAsync()
        {
            var bankListQuery = new BankListQuery();
            var response = await _mediator.Send(bankListQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<BalanceEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<BalanceEnquiryResponse>), 400)]
        [HttpGet, Route("get-wallet-balance/{accountNumber}")]
        public async Task<IActionResult> GetAccountBalanceAsync(string accountNumber)
        {
            var balanceEnquiryQuery = new BalanceEnquiryQuery(accountNumber);
            var response = await _mediator.Send(balanceEnquiryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<decimal>), 200)]
        [ProducesResponseType(typeof(ResponseModel<decimal>), 400)]
        [HttpGet, Route("transaction-fee/{amount}/{transactionType}")]
        public async Task<IActionResult> GetTransactionFeeAsync(decimal amount, TransactionType transactionType)
        {
            var getFeeQuery = new GetFeeQuery(transactionType, amount);
            var response = await _mediator.Send(getFeeQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion


        #region Wallet Queries
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<NipNameEnquiryResponse>), 400)]
        [HttpGet, Route("wallet/local-transfer-name-enquiry/{phoneNumber}")]
        public async Task<IActionResult> LocalWalletTransferNameEnquiryAsync(string phoneNumber)
        {
            var localTransferNameEnquiryQuery = new LocalWalletTransferNameEnquiryQuery(phoneNumber);
            var response = await _mediator.Send(localTransferNameEnquiryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 400)]
        [HttpGet, Route("wallet/get-banks")]
        public async Task<IActionResult> GetWalletBanksAsync()
        {
            var bankListQuery = new WalletBankListQuery();
            var response = await _mediator.Send(bankListQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [ProducesResponseType(typeof(ResponseModel<BalanceEnquiryResponse>), 200)]
        [ProducesResponseType(typeof(ResponseModel<BalanceEnquiryResponse>), 400)]
        [HttpGet, Route("wallet/get-wallet-balance/{accountNumber}")]
        public async Task<IActionResult> GetWalletAccountBalanceAsync(string accountNumber)
        {
            var balanceEnquiryQuery = new WalletBalanceEnquiryQuery(accountNumber);
            var response = await _mediator.Send(balanceEnquiryQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<List<NipBank>>), 400)]
        [HttpGet, Route("update")]
        public async Task<IActionResult> update()
        {
            await _transferService.UpdateWallet();
            var bankListQuery = new BankListQuery();
            var response = await _mediator.Send(bankListQuery);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
