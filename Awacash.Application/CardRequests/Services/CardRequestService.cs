using AutoMapper;
using Awacash.Application.CardRequestConfigurations.Services;
using Awacash.Application.CardRequests.DTOs;
using Awacash.Application.CardRequests.FilterModels;
using Awacash.Application.CardRequests.Specifications;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Specifications;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Extentions;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Domain.Models.Transactions;
using Awacash.Domain.Settings;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Awacash.Domain.Common.Errors.Errors;

namespace Awacash.Application.CardRequests.Services
{
    public class CardRequestService : ICardRequestService
    {
        private readonly ILogger<CardRequestService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IAwacashThirdPartyService _berachahThirdPartyService;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly AppSettings _appSettings;
        private readonly ICryptoService _cryptoService;
        public CardRequestService(ILogger<CardRequestService> logger, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IMapper mapper, ICurrentUser currentUser, IAwacashThirdPartyService berachahThirdPartyService, IBankOneAccountService bankOneAccountService, ICryptoService cryptoService, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _currentUser = currentUser;
            _berachahThirdPartyService = berachahThirdPartyService;
            _bankOneAccountService = bankOneAccountService;
            _cryptoService = cryptoService;
            _appSettings = appSettings.Value;
        }
        public async Task<ResponseModel<bool>> CreateCardRequestAsync(string accountNumber, string cardName, CardType cardType, string deliveryAddress, string cardConfigId)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure($"Customer not found");
                }
                if (string.IsNullOrWhiteSpace(customer.AccountId))
                {
                    return ResponseModel<bool>.Failure("Customer account not found");
                }

                var cardRequest = await _unitOfWork.CardRequestRepository.GetByAsync(x => x.Id == customerId && x.CardType == cardType, x => x.Customer);
                if (cardRequest is not null)
                {
                    return ResponseModel<bool>.Failure($"you have an active request, Please wait, or select a different card type");
                }
                var cardConfig = await _unitOfWork.CardRequestConfigurationtRepository.GetByAsync(x => x.Id == cardConfigId);
                if (cardConfig is null)
                {
                    return ResponseModel<bool>.Failure($"Card request configuration not found");
                }


                var accountsResponse = await _bankOneAccountService.GetAccountsByCustomerId(customer.AccountId);
                if (accountsResponse == null || accountsResponse.Data == null)
                {
                    return ResponseModel<bool>.Failure("Fail to get account details. Please try again");
                }

                if (!accountsResponse.Data.Accounts.Any(x => x.Nuban == accountNumber))
                {
                    return ResponseModel<bool>.Failure("Invalid Customer account. Please try again");
                }

                var balanceResponse = await _bankOneAccountService.BalanceEnquiry(accountNumber);
                if (balanceResponse == null || balanceResponse.Data == null)
                {
                    return ResponseModel<bool>.Failure("Fail to get account balance. Please try again");
                }


                if (balanceResponse.Data.WithdrawableAmount < cardConfig.Price)
                {
                    return ResponseModel<bool>.Failure($"Insufficient fund");
                }

                var transref = $"2528{_cryptoService.GenerateNumericKey(8)}";
                var debitres = await _bankOneAccountService.Debit(cardConfig.Price, 0, accountNumber, _appSettings.DebitGL, "Card Request", "Mobile", transref);
                if (debitres == null || debitres.Data == null)
                {
                    return ResponseModel<bool>.Failure("Fail to complete transaction");
                }

                //wallet.Balance -= cardConfig.Price;
                //wallet.ModifiedBy = _currentUser.Name;
                //wallet.ModifiedByIp = "::1";
                //wallet.ModifiedDate = _dateTimeProvider.UtcNow;
                //wallet.CheckSum = wallet.GetCheckSum();
                //_unitOfWork.WalletRepository.Update(wallet);

                var newCardRequest = new CardRequest
                {
                    CardName = cardName,
                    CardType = cardType,
                    CreatedBy = _currentUser.Name,
                    CreatedDate = _dateTimeProvider.UtcNow,
                    DeliveryAddress = deliveryAddress,
                    CreatedByIp = "::1",
                    IsDeleted = false,
                    CustomerId = customerId,
                    DeliveryStatus = CardDeliveryStatus.Pending
                };
                _unitOfWork.CardRequestRepository.Add(newCardRequest);
                var cardRequestTransaction = new Transaction()
                {
                    CustomerId = customerId,
                    Amount = cardConfig.Price,
                    DebitAccountName = customer.FullName,
                    Fee = 0,
                    Narration = "Card Request",
                    Currency = "NGN",
                    TransactionReference = transref,
                    TransactionType = TransactionType.CardRequest,
                    RecordType = RecordType.Debit

                };

                _unitOfWork.TransactionRepository.Add(cardRequestTransaction);
                await _unitOfWork.Complete();

                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.CardRequestConfirmation);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[DELIVERY_DURATION]", "2 Weeks"));
                }

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestAsync));
                return ResponseModel<bool>.Failure($"error occured while creating card request congiguration");
            }
        }

        public async Task<ResponseModel<bool>> WalletCreateCardRequestAsync(string cardName, CardType cardType, string deliveryAddress, string cardConfigId)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
                if (customer is null)
                {
                    return ResponseModel<bool>.Failure($"Customer not found");
                }
                var cardRequest = await _unitOfWork.CardRequestRepository.GetByAsync(x => x.Id == customerId && x.CardType == cardType, x => x.Customer);
                if (cardRequest is not null)
                {
                    return ResponseModel<bool>.Failure($"you have an active request, Please wait, or select a different card type");
                }
                var cardConfig = await _unitOfWork.CardRequestConfigurationtRepository.GetByAsync(x => x.Id == cardConfigId);
                if (cardConfig is null)
                {
                    return ResponseModel<bool>.Failure($"Card request configuration not found");
                }

                var wallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customerId);
                if (wallet is null)
                {
                    return ResponseModel<bool>.Failure($"Customer wallet not found");
                }

                if (!wallet.ValidateCheckSum())
                {
                    return ResponseModel<bool>.Failure("Suspected fraud on debit wallet, Please contact customer care");
                }

                if (wallet.Balance < cardConfig.Price)
                {
                    return ResponseModel<bool>.Failure($"Insufficient fund");
                }

                wallet.Balance -= cardConfig.Price;
                wallet.ModifiedBy = _currentUser.Name;
                wallet.ModifiedByIp = "::1";
                wallet.ModifiedDate = _dateTimeProvider.UtcNow;
                wallet.CheckSum = wallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(wallet);

                var newCardRequest = new CardRequest
                {
                    CardName = cardName,
                    CardType = cardType,
                    CreatedBy = _currentUser.Name,
                    CreatedDate = _dateTimeProvider.UtcNow,
                    DeliveryAddress = deliveryAddress,
                    CreatedByIp = "::1",
                    IsDeleted = false,
                    CustomerId = customerId,
                    DeliveryStatus = CardDeliveryStatus.Pending
                };
                _unitOfWork.CardRequestRepository.Add(newCardRequest);
                var cardRequestTransaction = new Transaction()
                {
                    CustomerId = customerId,
                    Amount = cardConfig.Price,
                    DebitAccountName = customer.FullName,
                    Fee = 0,
                    Narration = "Card Request",
                    Currency = "NGN",
                    TransactionReference = _dateTimeProvider.UtcNow.Millisecond.ToString(),
                    TransactionType = TransactionType.CardRequest,
                    RecordType = RecordType.Debit

                };

                _unitOfWork.TransactionRepository.Add(cardRequestTransaction);
                await _unitOfWork.Complete();

                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.CardRequestConfirmation);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[DELIVERY_DURATION]", "2 Weeks"));
                }

                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(CreateCardRequestAsync));
                return ResponseModel<bool>.Failure($"error occured while creating card request congiguration");
            }
        }

        public async Task<ResponseModel<List<CardRequestDTO>>> GetAllCardRequestAsync()
        {
            try
            {
                var cardRequest = await _unitOfWork.CardRequestRepository.ListAllAsync(x => x.Customer);
                return ResponseModel<List<CardRequestDTO>>.Success(_mapper.Map<List<CardRequestDTO>>(cardRequest));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(GetAllCardRequestAsync));
                return ResponseModel<List<CardRequestDTO>>.Failure($"error occured while fetching card request congiguration");
            }
        }

        public async Task<ResponseModel<CardRequestDTO>> GetCardRequestByIdAsync(string id)
        {
            try
            {
                var cardRequest = await _unitOfWork.CardRequestRepository.GetByAsync(x => x.Id == id, x => x.Customer);
                return ResponseModel<CardRequestDTO>.Success(_mapper.Map<CardRequestDTO>(cardRequest));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message, nameof(GetAllCardRequestAsync));
                return ResponseModel<CardRequestDTO>.Failure($"error occured while fetching card request congiguration");
            }
        }

        public async Task<ResponseModel<PagedResult<CardRequestDTO>>> GetPagedCardRequestAsync(CardRequestFilterModel filterModel)
        {
            try
            {
                var cardRequestFilterSpecification = new CardRequestFilterSpecification(cardName: filterModel.CardName, cardType: filterModel.CardType, deliveryStatus: filterModel.DeliveryStatus);
                var cardRequests = await _unitOfWork.CardRequestRepository.ListAsync(filterModel.PageIndex, filterModel.PageSize, cardRequestFilterSpecification);
                return ResponseModel<PagedResult<CardRequestDTO>>.Success(_mapper.Map<PagedResult<CardRequestDTO>>(cardRequests));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting card requests: {ex.Message}", nameof(GetPagedCardRequestAsync));
                return ResponseModel<PagedResult<CardRequestDTO>>.Failure("Exception error");
            }
        }
    }
}
