using System;
using System.Net.NetworkInformation;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Enums;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.Loan;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Microsoft.Extensions.Logging;

namespace Awacash.Application.Loans.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILogger<LoanService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly ILoanProviderService _loanProviderService;
        private readonly ICryptoService _cryptoService;
        public LoanService(ILogger<LoanService> logger, IUnitOfWork unitOfWork, ICurrentUser currentUser, IDateTimeProvider dateTimeProvider, IBankOneAccountService bankOneAccountService, ICryptoService cryptoService, ILoanProviderService loanProviderService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _dateTimeProvider = dateTimeProvider;
            _bankOneAccountService = bankOneAccountService;
            _cryptoService = cryptoService;
            _loanProviderService = loanProviderService;
        }

        public async Task<ResponseModel> CreateLoanRequest(int amount, string account, string bvn, LoanDuration duration, LoanType loanType, string placeOfEmployment, decimal monthlySalary, EmploymentStatus employmentStatus, string pin)
        {

            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel.Failure("Customer not found");
            }

            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel.Failure("Customer not found");
            }
            var empSatus = "";

            switch (employmentStatus)
            {
                case EmploymentStatus.Employed:
                    empSatus = "E";
                    break;
                case EmploymentStatus.UnEmployed:
                    empSatus = "UE";
                    break;
                case EmploymentStatus.SelfEmployed:
                    empSatus = "SE";
                    break;
                default:
                    break;
            }

            var tenor = 0;
            switch (duration)
            {
                case LoanDuration.DAYS_180:
                    tenor = 180;
                    break;
                case LoanDuration.DAYS_90:
                    tenor = 90;
                    break;
                default:
                    tenor = 30;
                    break;
            }

            if (customer.PinTries >= 5)
            {
                return ResponseModel<NipTransferResponse>.Failure("Your transaction pin has been blocked");
            }

            if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, pin) != customer.SaltedHashedPin)
            {
                customer.PinTries += 1;
                customer.ModifiedBy = customer.FullName;
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                customer.ModifiedByIp = "::0";
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();
                return ResponseModel<NipTransferResponse>.Failure("Invalid transaction pin");
            }

            var request = new CreateLoanRequest
            {
                AccountNumber = account,
                Bvn = bvn,
                EmploymentStatus = empSatus,
                Amount = amount,
                Country = customer.Country,
                CustomerId = "007057", //customer.AccountId,
                FirstName = "ADEYINKA", //customer.FirstName,
                LastName = "OKUNFOLAMI", //customer.LastName,
                Gender = "Female",
                DateOfBirth = DateTime.Now.AddYears(-25),
                Tenor = tenor,
                TenorType = "DAYS",
                LoanType = loanType == LoanType.AwaQuick ? "AwaQuick" : "AwaTerm",
                MariratalStatus = "",
                MiddleName = "RITA", //customer.MiddleName,
                MobileNumber = customer.PhoneNumber,
                Nationality = "Nigerian",
                PrimaryAddress = customer.Address,
                PrimaryCityLGA = customer.Country,
                State = customer.State,
                TransactionPin = "1234",
            };
            var loanProviderResponse = await _loanProviderService.CreateLoan(request);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                if (loanProviderResponse.Data.isSuccessful)
                {
                    return ResponseModel.Success(loanProviderResponse.Data.message);
                };
            }
            return ResponseModel.Failure(loanProviderResponse.Data.message);
        }

        public async Task<ResponseModel> RepayLoanRequest(decimal amount, string accountNumber, string transactionPin, bool isTermination)
        {

            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel.Failure("");
            }

            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel.Failure("Customer not found");
            }

            if (customer.PinTries >= 5)
            {
                return ResponseModel.Failure("Your transaction pin has been blocked");
            }

            var sourceBalance = await _bankOneAccountService.BalanceEnquiry(accountNumber);
            if (sourceBalance == null || sourceBalance.Data == null)
            {
                return ResponseModel.Failure("network error, please try again");

            }
            var finalAmount = amount;

            var loanBalanceResponse = await _loanProviderService.GetLoanBalance(customer.AccountId);
            if (loanBalanceResponse is null || loanBalanceResponse.Data is null)
            {
                return ResponseModel.Failure("network error, please try again");
            }

            if (isTermination)
            {
                finalAmount = loanBalanceResponse.Data[0].totalOutstandingAmount;
            }


            if (sourceBalance.Data.AvailableBalance < finalAmount)
            {
                return ResponseModel<NipTransferResponse>.Failure("Insufficient fund");
            }
            if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, transactionPin) != customer.SaltedHashedPin)
            {
                customer.PinTries += 1;
                customer.ModifiedBy = customer.FullName;
                customer.ModifiedDate = _dateTimeProvider.UtcNow;
                customer.ModifiedByIp = "::0";
                _unitOfWork.CustomerRepository.Update(customer);
                await _unitOfWork.Complete();
                return ResponseModel<NipTransferResponse>.Failure("Invalid transaction pin");
            }

            var request = new LoanRepaymentRequest
            {
                accountNumber = accountNumber,
                amount = amount,
                feeNarration = "",
                interestNarration = "",
                isTermination = isTermination,
                principalNarration = "",
                transactionPin = "1234"

            };

            var loanProviderResponse = await _loanProviderService.RepayLoan(request);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                if (loanProviderResponse.Data.isSuccessful)
                {
                    return ResponseModel.Success(loanProviderResponse.Data.message);
                };
            }
            return ResponseModel.Failure(loanProviderResponse.Data.message);
        }

        public async Task<ResponseModel<List<LoanStatusModel>>> GetCustomerLoanByStatus(string status)
        {
            List<LoanStatusModel> customerLoans = new();
            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel<List<LoanStatusModel>>.Failure("");
            }

            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel<List<LoanStatusModel>>.Failure("Customer not found");
            }

            var loanProviderResponse = await _loanProviderService.GetLoansByStatus(status);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                customerLoans = loanProviderResponse.Data.Where(x => x.customerId == customer.AccountId).ToList();
            }

            return ResponseModel<List<LoanStatusModel>>.Success(customerLoans);
        }

        public async Task<ResponseModel<List<LoanModel>>> GetLoanByCustomerId()
        {
            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel<List<LoanModel>>.Failure("");
            }
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel<List<LoanModel>>.Failure("Customer not found");
            }

            var loanProviderResponse = await _loanProviderService.GetLoanByCustomerId(customer.AccountId);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                return ResponseModel<List<LoanModel>>.Success(loanProviderResponse.Data);
            }

            return ResponseModel<List<LoanModel>>.Failure("Failed to fetch loan");

        }

        public async Task<ResponseModel<List<LoanBalanceModel>>> GetCustomerLoanBalance()
        {
            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel<List<LoanBalanceModel>>.Failure("");
            }
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel<List<LoanBalanceModel>>.Failure("Customer not found");
            }

            var loanProviderResponse = await _loanProviderService.GetLoanBalance(customer.AccountId);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                return ResponseModel<List<LoanBalanceModel>>.Success(loanProviderResponse.Data);
            }

            return ResponseModel<List<LoanBalanceModel>>.Failure("Failed to fetch loan balance");
        }

        public async Task<ResponseModel<LoanRepaymentModel>> GetCustomerTotalLoanRepayment(string accountNumber)
        {
            var customerId = _currentUser.GetCustomerId();
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return ResponseModel<LoanRepaymentModel>.Failure("");
            }
            var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId);
            if (customer is null)
            {
                return ResponseModel<LoanRepaymentModel>.Failure("Customer not found");
            }

            var loanProviderResponse = await _loanProviderService.GetLoanTotalRepayment(accountNumber);
            if (loanProviderResponse != null && loanProviderResponse.Data != null)
            {
                return ResponseModel<LoanRepaymentModel>.Success(loanProviderResponse.Data);
            }

            return ResponseModel<LoanRepaymentModel>.Failure("Failed to fetch loan balance");
        }


    }
}

