using AutoMapper;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Role.DTOs;
using Awacash.Application.Role.Specifications;
using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.FilterModels;
using Awacash.Application.Transactions.Specifications;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Awacash.Domain.Common.Errors.Errors;

namespace Awacash.Application.Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwacashThirdPartyService _BerachahThirdPartyService;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly ICurrentUser _currentUser;
        private readonly ICryptoService _cryptoService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        public TransactionService(ILogger<TransactionService> logger, IUnitOfWork unitOfWork, IAwacashThirdPartyService BerachahThirdPartyService, ICurrentUser currentUser, ICryptoService cryptoService, IDateTimeProvider dateTimeProvider, IMapper mapper, IBankOneAccountService bankOneAccountService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _BerachahThirdPartyService = BerachahThirdPartyService;
            _currentUser = currentUser;
            _cryptoService = cryptoService;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _bankOneAccountService = bankOneAccountService;
        }


        public async Task<ResponseModel<string>> SaveTransactionNotification(string originatoraccountnumber, string originatorname, string amount, string craccountname, string craccount, string paymentreference, string bankname, string bankcode, string sessionid, string narration)
        {
            try
            {
                var transRef = _dateTimeProvider.UtcNow.ToString("yyMMddHHmmss");
                var transNotification = await _unitOfWork.AccountTransactionNotificationRepository.GetByAsync(x => x.Paymentreference == paymentreference && x.Sessionid == sessionid);
                if (transNotification != null)
                {
                    return ResponseModel<string>.Success(transRef);
                }

                var transactionNotification = new AccountTransactionNotification()
                {
                    Originatoraccountnumber = originatoraccountnumber,
                    Originatorname = originatorname,
                    Amount = amount,
                    Craccount = craccount,
                    Paymentreference = paymentreference,
                    Bankname = bankname,
                    Bankcode = bankcode,
                    Sessionid = sessionid,
                    Narration = narration,
                    Craccountname = craccountname,
                    IsVerify = false,
                    CreatedBy = "sys",
                    CreatedDate = _dateTimeProvider.UtcNow,
                    TransactionReference = transRef

                };
                _unitOfWork.AccountTransactionNotificationRepository.Add(transactionNotification);
                await _unitOfWork.Complete();
                return ResponseModel<string>.Success(transRef);
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<string>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<List<TransactionDTO>>> GetCustomerTransactions()
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var transactions = await _unitOfWork.TransactionRepository.ListAsync(x => x.CustomerId == customerId);
                var transactionDtos = _mapper.Map<List<TransactionDTO>>(transactions);
                return ResponseModel<List<TransactionDTO>>.Success(transactionDtos);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<TransactionDTO>>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<TransactionDTO>> GetTransactionById(string id)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
                var transactionDtos = _mapper.Map<TransactionDTO>(transactions);
                return ResponseModel<TransactionDTO>.Success(transactionDtos);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<TransactionDTO>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<PagedResult<TransactionDTO>>> GetPaginatedTransactions(TransactionFilterModel filter)
        {
            try
            {
                //var customerId = _currentUser.GetCustomerId();
                var transactionSpecification = new TransactionFilterSpecification(filter.CustomerId, filter.StartDate, filter.EndDate, filter.TransactionType, filter.RecordType, filter.AccountNumber, filter.OrderBy, filter.ByDescending);
                var pagedTransactions = await _unitOfWork.TransactionRepository.ListAsync(filter.PageIndex, filter.PageSize, transactionSpecification);

                return ResponseModel<PagedResult<TransactionDTO>>.Success(_mapper.Map<PagedResult<TransactionDTO>>(pagedTransactions));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting paginated transaction: {ex.Message}", nameof(GetPaginatedTransactions));
                return ResponseModel<PagedResult<TransactionDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<TransactionResponseDto>>> GetCoreTransactions(string accountNumber, DateTime startDate, DateTime endDate)
        {
            try
            {
                //var customerId = _currentUser.GetCustomerId(); 
                return await _bankOneAccountService.GetTransactions(accountNumber, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting  transaction: {ex.Message}", nameof(GetPaginatedTransactions));
                return ResponseModel<List<TransactionResponseDto>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<TransactionQuery>>> TransQuery(string crcaccount)
        {
            try
            {
                var response = await _BerachahThirdPartyService.TransQuery(crcaccount);
                if (response.IsSuccessful)
                {
                    return ResponseModel<List<TransactionQuery>>.Failure(response.Message);
                }

                return ResponseModel<List<TransactionQuery>>.Success(response?.Data?.Transactions);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<TransactionQuery>>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<WalletBalanceDto>> GetWalletBalance()
        {
            try
            {
                WalletBalanceDto walletBalance = new WalletBalanceDto();
                var response = await _BerachahThirdPartyService.GetCollectionBanlance("123456");
                if (response != null && !string.IsNullOrWhiteSpace(response.Message))
                {
                    decimal collectionBal = 0;
                    if (decimal.TryParse(response.Message, out collectionBal))
                    {
                        walletBalance.CollectionBalance = collectionBal;
                    }
                }
                var walletsBal = (await _unitOfWork.WalletRepository.ListAllAsync())?.ToList()?.Sum(x => x.Balance);
                if (walletsBal != null && walletsBal.HasValue)
                {
                    walletBalance.TotalWalletBalance = walletsBal.Value;

                }
                return ResponseModel<WalletBalanceDto>.Success(walletBalance);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<WalletBalanceDto>.Failure("Error occure please try again later");
            }
        }
    }
}
