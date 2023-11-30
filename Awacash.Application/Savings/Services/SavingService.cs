using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Customers.DTOs;
using Awacash.Domain.Enums;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awacash.Application.Savings.DTOs;
using Awacash.Application.Common.Interfaces.Services;
using AutoMapper;
using Awacash.Domain.Common.Constants;

namespace Awacash.Application.Savings.Services
{
    public class SavingService : ISavingService
    {
        private readonly ILogger<SavingService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly ICryptoService _cryptoService;
        public SavingService(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<SavingService> logger, IDateTimeProvider dateTimeProvider, IMapper mapper, IBankOneAccountService bankOneAccountService, ICryptoService cryptoService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _bankOneAccountService = bankOneAccountService;
            _cryptoService = cryptoService;
        }

        public async Task<ResponseModel<SavingDTO>> CreateSavingAsync(string reason, decimal targetAmount, DeductionFrequency deductionFrequency, string savingConfigId, SavingsDuration SavingsDuration, string accountNumber)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == _currentUser.GetCustomerId());
                if (customer == null) return ResponseModel<SavingDTO>.Failure("Customer not found");

                var savingConfig = await _unitOfWork.SavingConfigurationRepository.GetByIdAsync(savingConfigId);
                if (savingConfig is null) return ResponseModel<SavingDTO>.Failure("Savings Configuration not found");

                int planDuration = 0;

                switch (SavingsDuration)
                {
                    case SavingsDuration.Months_3:
                        planDuration = 90;
                        break;
                    case SavingsDuration.Months_6:
                        planDuration = 180;
                        break;
                    default:
                        planDuration = 365;
                        break;
                }
                var (deductionAmount, endDate) = CalDeductionAmountAndEndDate(targetAmount, deductionFrequency, planDuration);

                if (deductionAmount <= 0) return ResponseModel<SavingDTO>.Failure("Deduction amount must be greater than zero");


                var deductionAccountNameEnquiry = await _bankOneAccountService.GetAccountsByAccountNumber(accountNumber);
                if (deductionAccountNameEnquiry == null || deductionAccountNameEnquiry.Data == null || !deductionAccountNameEnquiry.IsSuccessful)
                {
                    return ResponseModel<SavingDTO>.Failure("Invalid debit account number, Please try again");
                }

                if (customer.AccountId != deductionAccountNameEnquiry.Data.CustomerID)
                {
                    return ResponseModel<SavingDTO>.Failure("Invalid customer account number, Please try again");
                }

                var balanceRes = await _bankOneAccountService.BalanceEnquiry(accountNumber);
                if (balanceRes == null || !balanceRes.IsSuccessful || balanceRes.Data == null)
                {
                    return ResponseModel<SavingDTO>.Failure("Fail to get balance, please try again later");
                }
                string createdAccount = string.Empty;
                if (savingConfig.SavingType == SavingType.AWAFIXED)
                {

                    if (balanceRes.Data.WithdrawableAmount < targetAmount)
                    {
                        return ResponseModel<SavingDTO>.Failure("Insuffient balance");
                    }

                    var fixedResponse = await _bankOneAccountService.CreateFixDeposit(customer.AccountId, true, (int)decimal.Parse(savingConfig.PlanInterestRate.ToString()), targetAmount.ToString(), planDuration, accountNumber, true, true, true);
                    if (fixedResponse == null || fixedResponse.Data == null)
                    {
                        return ResponseModel<SavingDTO>.Failure("Fail to create savings, please try again later");
                    }


                }
                bool debited = false;

                if (savingConfig.SavingType != SavingType.AWAFIXED)
                {
                    if (balanceRes.Data.WithdrawableAmount < Math.Round(deductionAmount, 2))
                    {
                        return ResponseModel<SavingDTO>.Failure("Insuffient balance");
                    }
                    var savingAcctResponse = await _bankOneAccountService.AddAccount(customer.AccountId, savingConfig.ProductCode, customer.Email, $"{customer.LastName} {customer.FirstName} {customer.MiddleName}");
                    if (savingAcctResponse == null || savingAcctResponse.Data == null)
                    {
                        return ResponseModel<SavingDTO>.Failure("Fail to create savings, please try again later");
                    }
                    if (!savingAcctResponse.Data.IsSuccessful)
                    {
                        return ResponseModel<SavingDTO>.Failure($"{savingAcctResponse.Data.Message ?? "Fail to create savings, please try again later"}");
                    }
                    createdAccount = savingAcctResponse.Data.AccountNumber;

                    if (!string.IsNullOrWhiteSpace(createdAccount))
                    {

                        var tranRef = _cryptoService.GenerateNumericKey(12);

                        var response = await _bankOneAccountService.IntraBankTransfer(Math.Round(deductionAmount, 2), accountNumber, createdAccount, "Initial savings credit", "mobile", tranRef);

                        if (response != null && response.IsSuccessful && response.Data != null)
                        {
                            if (response.Data.IsSuccessful)
                            {
                                debited = true;

                                var debitTransaction = new Transaction()
                                {
                                    CustomerId = customer.Id,
                                    Amount = Math.Round(deductionAmount, 2),
                                    DebitAccountName = deductionAccountNameEnquiry.Data.Name,
                                    DebitAccountNumber = accountNumber,
                                    CreditAccountName = $"{customer.LastName} {customer.FirstName} {customer.MiddleName}",
                                    CreditAccountNumber = createdAccount,
                                    Fee = 0,
                                    Narration = "savings",
                                    Currency = "NGN",
                                    TransactionReference = tranRef,
                                    TransactionType = TransactionType.Saving,
                                    RecordType = RecordType.Debit,
                                    Status = response.Data.IsSuccessful ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED
                                };

                                _unitOfWork.TransactionRepository.Add(debitTransaction);

                            }
                        }

                        if (savingConfig.SavingType == SavingType.AWATARGET)
                        {
                            var makePndRes = await _bankOneAccountService.ActivatePND(createdAccount);

                        }
                    }


                }




                var nextPaymentDate = CalNextPayment(deductionFrequency);
                var saving = new Saving
                {
                    DeductionFrequency = deductionFrequency,
                    Duration = planDuration,
                    TargetAmount = targetAmount,
                    InterestRate = savingConfig.PlanInterestRate,
                    Reason = reason,
                    Balance = 0,
                    DeductionaAmount = Math.Round(deductionAmount, 2),
                    MaturityDate = endDate,
                    CreatedBy = _currentUser.GetCustomerId(),
                    CreatedDate = _dateTimeProvider.UtcNow,
                    CreatedByIp = "::1",
                    CustomerId = customer.Id,
                    SavingType = savingConfig.SavingType,
                    DeductionAccount = accountNumber,
                    SavingAccount = createdAccount,
                    NextPaymentDate = debited ? nextPaymentDate : null,
                    PaymentDate = nextPaymentDate

                };
                _unitOfWork.SavingRepository.Add(saving);
                await _unitOfWork.Complete();

                var savingDto = _mapper.Map<SavingDTO>(saving);
                return ResponseModel<SavingDTO>.Success(savingDto);

            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while creating saving: {ex.Message}", nameof(CreateSavingAsync));
                return ResponseModel<SavingDTO>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<SavingDTO>>> GetAllSavingsAsync()
        {
            try
            {
                var savings = await _unitOfWork.SavingRepository.ListAllAsync();
                return ResponseModel<List<SavingDTO>>.Success(_mapper.Map<List<SavingDTO>>(savings));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting saving: {ex.Message}", nameof(GetAllSavingsAsync));
                return ResponseModel<List<SavingDTO>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<SavingDTO>>> GetAllSavingsByIdAsync()
        {
            try
            {
                var id = _currentUser.GetCustomerId();
                var savings = await _unitOfWork.SavingRepository.ListAsync(x => x.CustomerId == id);
                return ResponseModel<List<SavingDTO>>.Success(_mapper.Map<List<SavingDTO>>(savings));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting all savings by customer id: {ex.Message}", nameof(GetAllSavingsByIdAsync));
                return ResponseModel<List<SavingDTO>>.Failure("Exception error");
            }
        }

        public Task<ResponseModel<PagedResult<SavingDTO>>> GetPaginatedSavingAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<SavingDTO>> GetSavingByIdAsync(string id)
        {
            try
            {
                var savings = await _unitOfWork.SavingRepository.GetByAsync(x => x.Id == id);
                return ResponseModel<SavingDTO>.Success(_mapper.Map<SavingDTO>(savings));
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception occured while getting saving by customer id: {ex.Message}", nameof(GetSavingByIdAsync));
                return ResponseModel<SavingDTO>.Failure("Exception error");
            }
        }


        #region privatemethod
        private Tuple<decimal, DateTime> CalDeductionAmountAndEndDate(decimal targetAmount, DeductionFrequency deductionFrequency, int duration)
        {
            decimal amount = 0;
            DateTime endDate = DateTime.UtcNow;

            if (deductionFrequency == DeductionFrequency.Daily)
            {
                amount = Math.Ceiling((targetAmount / duration) * 100) / 100;
                endDate = _dateTimeProvider.UtcNow.AddDays(duration);
            }
            else if (deductionFrequency == DeductionFrequency.Weekly)
            {
                var no0fDeduction = duration / 7;
                amount = Math.Ceiling((targetAmount / no0fDeduction) * 100) / 100;
                endDate = _dateTimeProvider.UtcNow.AddDays(no0fDeduction);
            }
            else if (deductionFrequency == DeductionFrequency.Monthly)
            {
                var no0fDeduction = duration / 30;
                amount = Math.Ceiling((targetAmount / no0fDeduction) * 100) / 100;
                endDate = _dateTimeProvider.UtcNow.AddMonths(no0fDeduction);
            }
            return new Tuple<decimal, DateTime>(amount, endDate);
        }

        private static DateTime CalNextPayment(DeductionFrequency deductionFrequency)
        {
            DateTime nextPaymentDate = DateTime.UtcNow;

            if (deductionFrequency == DeductionFrequency.Daily)
            {

                nextPaymentDate = DateTime.UtcNow.AddDays(1);
            }
            else if (deductionFrequency == DeductionFrequency.Weekly)
            {
                nextPaymentDate = DateTime.UtcNow.AddDays(7);
            }
            else if (deductionFrequency == DeductionFrequency.Monthly)
            {
                nextPaymentDate = DateTime.UtcNow.AddMonths(1);
            }
            return nextPaymentDate;
        }

        #endregion

    }
}
