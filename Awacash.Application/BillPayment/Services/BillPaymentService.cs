using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Extentions;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Domain.Models.Transactions;
using Awacash.Domain.Settings;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Services
{
    public class BillPaymentService : IBillPaymentService
    {
        private readonly IAwacashThirdPartyService _berachahThirdPartyService;
        private readonly ILogger<BillPaymentService> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptoService _cryptoService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBankOneAccountService _bankOneAccountService;
        private readonly AppSettings _appSettings;

        private List<string> _interswtichSuccessCode;

        public BillPaymentService(IAwacashThirdPartyService berachahThirdPartyService, ILogger<BillPaymentService> logger, ICurrentUser currentUser, IUnitOfWork unitOfWork, ICryptoService cryptoService, IDateTimeProvider dateTimeProvider, IBankOneAccountService bankOneAccountService, IOptions<AppSettings> appSettings)
        {
            _berachahThirdPartyService = berachahThirdPartyService;
            _logger = logger;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _cryptoService = cryptoService;
            _dateTimeProvider = dateTimeProvider;
            _bankOneAccountService = bankOneAccountService;
            _appSettings = appSettings.Value;
            _interswtichSuccessCode = new List<string> { "90010", "90011", "90016", "90000" };
        }

        public async Task<ResponseModel<List<Biller>>> GetBillerByCategory(int categoryId)
        {
            List<string> airTimeItemToRemove = new List<string>() { "617", "653", "903" };
            List<string> dataItemToRemove = new List<string>() { "102", "2554", "2513" };
            try
            {
                var response = await _berachahThirdPartyService.GetBillerByCategory(categoryId);

                if (response.IsSuccessful && response?.Data?.Count > 0)
                {
                    if (categoryId == 3)
                    {
                        var selectedItems = response.Data.FindAll(x => !airTimeItemToRemove.Contains(x.Billerid));
                        return ResponseModel<List<Biller>>.Success(selectedItems.OrderBy(x => x.Billername).ToList(), response.Message);
                    }

                    if (categoryId == 4)
                    {

                        var selectedItems = response.Data.FindAll(x => !dataItemToRemove.Contains(x.Billerid));
                        var airtelRes = await _berachahThirdPartyService.GetBillerByCategory(63);
                        if (airtelRes.IsSuccessful && airtelRes?.Data?.Count > 0)
                        {
                            selectedItems.AddRange(airtelRes.Data);
                        }
                        return ResponseModel<List<Biller>>.Success(selectedItems.OrderBy(x => x.Billername).ToList(), response.Message);
                    }

                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillerByCategory));
                return ResponseModel<List<Biller>>.Failure("Exception error");
            }
        }



        public async Task<ResponseModel<List<Paymentitem>>> GetBillerPaymentItems(string billerId)
        {
            try
            {
                return await _berachahThirdPartyService.GetBillerPaymentItems(billerId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillerByCategory));
                return ResponseModel<List<Paymentitem>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<List<Biller>>> GetBillers()
        {
            try
            {
                return await _berachahThirdPartyService.GetBillers();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<List<Biller>>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<BillerCategory>> GetBillerCatrgory()
        {
            try
            {
                List<string> remove = new List<string>() { "15", "17", "20", "21", "24", "32", "69", "65", "62", "57", "47", "29", "26", "44", "49", "50", "55", "68", "66", "63" };
                var res = await _berachahThirdPartyService.GetBillerCategory();
                if (res != null && res.Data != null && res.Data.categorys.Count > 0)
                {
                    var categorys = new BillerCategory(res.Data.categorys.FindAll(x => !remove.Contains(x.Categoryid)).OrderBy(x => x.Categoryname).ToList());
                    return ResponseModel<BillerCategory>.Success(categorys);
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillerCatrgory));
                return ResponseModel<BillerCategory>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> SendPaymentAdvice(string accountNumber, string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string pin)
        {
            try
            {

                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == _currentUser.GetCustomerId());
                if (customer is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer not found");
                }
                if (string.IsNullOrWhiteSpace(customer.AccountId))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer account not found");
                }
                var accountsResponse = await _bankOneAccountService.GetAccountsByCustomerId(customer.AccountId);
                if (accountsResponse == null || accountsResponse.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to get account details. Please try again");
                }

                if (!accountsResponse.Data.Accounts.Any(x => x.Nuban == accountNumber))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid Customer account. Please try again");
                }

                var balanceResponse = await _bankOneAccountService.BalanceEnquiry(accountNumber);
                if (balanceResponse == null || balanceResponse.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to get account balance. Please try again");
                }



                if (customer.PinTries >= 5)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your transaction pin has been blocked");
                }

                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, pin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.FullName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid transaction pin");
                }

                var isValid = decimal.TryParse(amount, out decimal amt);

                if (!isValid)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid amount");
                }
                var finalamount = amt / 100;
                decimal fee = 0;
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.BillPayment && finalamount >= x.LowerBound && finalamount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                if (balanceResponse.Data.WithdrawableAmount < (finalamount + fee))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Insufficient fund");
                }

                var billCustomer = await _berachahThirdPartyService.ValidateCustomer(customerId, paymentCode);
                if (billCustomer == null || billCustomer.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }
                if (!billCustomer.IsSuccessful)
                {

                    return ResponseModel<PaymentAdviceResponse>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }

                if (!_interswtichSuccessCode.Contains(billCustomer.Data.ResponseCode))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Account does not exist, Please check and try again");
                }

                var transref = $"2528{_cryptoService.GenerateNumericKey(8)}";
                var debitres = await _bankOneAccountService.Debit(finalamount, fee, accountNumber, _appSettings.DebitGL, "Bills payment", "Mobile", transref);
                if (debitres == null || debitres.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to complete transaction");
                }


                var res = await _berachahThirdPartyService.SendPaymentAdvice(paymentCode, customerId, customerMobile, customerEmail, amount, transref);
                if (res == null || res.Data == null)
                {

                    //await _bankOneAccountService.Credit(finalamount + fee, 0, accountNumber, _appSettings.CreditGL, "Reversal", "Mobile", _cryptoService.GenerateNumericKey(12));
                    //return ResponseModel<PaymentAdviceResponse>.Failure("Transaction fail. Please try again later");
                }
                else if (!_interswtichSuccessCode.Contains(res.Data.ResponseCode))
                {
                    //await _bankOneAccountService.Credit(finalamount + fee, 0, accountNumber, _appSettings.CreditGL, "Reversal", "Mobile", _cryptoService.GenerateNumericKey(12));
                    //return ResponseModel<PaymentAdviceResponse>.Failure("Transaction fail. Please try again later");
                }

                var transaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = finalamount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    Currency = "NGN",
                    PaymentItemCode = paymentCode,
                    TransactionReference = res?.Data?.TransactionRef ?? transref,
                    TransactionType = TransactionType.BillPayment,
                    ResponseCode = res?.Data?.ResponseCode ?? "99",
                    RecordType = RecordType.Debit,
                    Fee = fee,
                    PaymentReference = res?.Data?.TransactionRef,
                    RechargePIN = res?.Data?.RechargePIN,
                    CustomerAddress = res?.Data?.AdditionalInfo?.CustomerAddress,
                    Status = (res.IsSuccessful && _interswtichSuccessCode.Contains(res?.Data?.ResponseCode)) ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED

                };
                _unitOfWork.TransactionRepository.Add(transaction);
                await _unitOfWork.Complete();
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", transaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", transaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", transaction.Narration).Replace("[ACCOUNT_BALANCE]", accountNumber).Replace("[TRANSACTION_TYPE]", "Bills Payment").Replace("[TRANSACTION_REFERENCE]", transaction.TransactionReference).Replace("[TIME]", transaction.CreatedDate.TimeOfDay.ToString()));
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<PaymentAdviceResponse>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> SendWalletPaymentAdvice(string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string pin)
        {
            try
            {

                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == _currentUser.GetCustomerId());
                if (customer is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer not found");
                }

                var wallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customer.Id);
                if (wallet is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Wallet not found");
                }

                if (!wallet.Status.Equals(WalletStatus.ACTIVE))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Account Blocked, Please contact customer care");
                }
                if (!wallet.ValidateCheckSum())
                {
                    wallet.Status = WalletStatus.INACTIVE;
                    wallet.ModifiedBy = customer.FullName;
                    wallet.ModifiedByIp = "::0";
                    wallet.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.WalletRepository.Update(wallet);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your account is temporarily blocked, Please contact customer care");
                }
                if (customer.PinTries >= 5)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your transaction pin has been blocked");
                }

                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, pin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.FullName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid transaction pin");
                }

                var isValid = decimal.TryParse(amount, out decimal amt);

                if (!isValid)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid amount");
                }
                var finalamount = amt / 100;
                decimal fee = 0;
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.BillPayment && finalamount >= x.LowerBound && finalamount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                if (wallet.Balance < (finalamount + fee))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Insufficient fund");
                }

                var billCustomer = await _berachahThirdPartyService.ValidateCustomer(customerId, paymentCode);


                if (billCustomer == null || billCustomer.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }
                if (!billCustomer.IsSuccessful)
                {

                    return ResponseModel<PaymentAdviceResponse>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }

                if (!_interswtichSuccessCode.Contains(billCustomer.Data.ResponseCode))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Account does not exist, Please check and try again");
                }

                //billCustomer.Data.
                wallet.Balance -= (finalamount + fee);
                wallet.ModifiedBy = customer.FullName;
                wallet.ModifiedDate = DateTime.UtcNow;
                wallet.CheckSum = wallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(wallet);

                var transref = $"2528{_cryptoService.GenerateNumericKey(8)}";
                var res = await _berachahThirdPartyService.SendPaymentAdvice(paymentCode, customerId, customerMobile, customerEmail, amount, transref);
                if (!res.IsSuccessful || res.Data == null)
                {
                    wallet.Balance += (finalamount + fee);
                    wallet.ModifiedBy = customer.FullName;
                    wallet.ModifiedDate = DateTime.UtcNow;
                    wallet.CheckSum = wallet.GetCheckSum();
                    _unitOfWork.WalletRepository.Update(wallet);
                    return ResponseModel<PaymentAdviceResponse>.Failure(res.Message);
                }
                //else if (res.Data != null && !(res.Data.ResponseCode.Contains("90000") || res.Data.ResponseCode.Contains("90010") || res.Data.ResponseCode.Contains("90011") || res.Data.ResponseCode.Contains("90016")))
                //{
                //    wallet.Balance += (finalamount + fee);
                //    wallet.ModifiedBy = customer.FullName;
                //    wallet.ModifiedDate = DateTime.UtcNow;
                //    wallet.CheckSum = wallet.GetCheckSum();
                //    _unitOfWork.WalletRepository.Update(wallet);


                //}

                var transaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = finalamount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    Currency = "NGN",
                    PaymentItemCode = paymentCode,
                    TransactionReference = res?.Data?.TransactionRef ?? transref,
                    TransactionType = TransactionType.BillPayment,
                    ResponseCode = res?.Data?.ResponseCode ?? "99",
                    RecordType = RecordType.Debit,
                    Fee = fee,
                    PaymentReference = res?.Data?.TransactionRef,
                    RechargePIN = res?.Data?.RechargePIN,
                    CustomerAddress = res?.Data?.AdditionalInfo?.CustomerAddress,
                    Status = (res.IsSuccessful && _interswtichSuccessCode.Contains(res?.Data?.ResponseCode)) ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED

                };
                _unitOfWork.TransactionRepository.Add(transaction);
                await _unitOfWork.Complete();
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", transaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", transaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", transaction.Narration).Replace("[ACCOUNT_BALANCE]", wallet.Balance.ToString()).Replace("[TRANSACTION_TYPE]", "Bills Payment").Replace("[TRANSACTION_REFERENCE]", transaction.TransactionReference).Replace("[TIME]", transaction.CreatedDate.TimeOfDay.ToString()));
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<PaymentAdviceResponse>.Failure("Exception error");
            }
        }


        public async Task<ResponseModel<BillPaymentCustomer>> ValidateCustomer(string customerId, string paymentCode)
        {
            try
            {
                var response = await _berachahThirdPartyService.ValidateCustomer(customerId, paymentCode);

                if (response == null || response.Data == null)
                {
                    return ResponseModel<BillPaymentCustomer>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }
                if (!response.IsSuccessful)
                {

                    return ResponseModel<BillPaymentCustomer>.Failure("Unable to validate customer, Please check customer detail or try again.");
                }

                if (!_interswtichSuccessCode.Contains(response.Data.ResponseCode))
                {
                    return ResponseModel<BillPaymentCustomer>.Failure("Account does not exist, Please check and try again");
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<BillPaymentCustomer>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> AirtimePurchase(string accountNumber, string paymentCode, string customerMobile, string amount, string pin)
        {
            try
            {

                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == _currentUser.GetCustomerId());
                if (customer is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer not found");
                }

                if (string.IsNullOrWhiteSpace(customer.AccountId))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer account not found");
                }
                var accountsResponse = await _bankOneAccountService.GetAccountsByCustomerId(customer.AccountId);
                if (accountsResponse == null || accountsResponse.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to get account details. Please try again");
                }

                if (!accountsResponse.Data.Accounts.Any(x => x.Nuban == accountNumber))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid Customer account. Please try again");
                }

                var balanceResponse = await _bankOneAccountService.BalanceEnquiry(accountNumber);
                if (balanceResponse == null || balanceResponse.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to get account balance. Please try again");
                }

                if (customer.PinTries >= 5)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your transaction pin has been blocked");
                }

                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, pin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.FullName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid transaction pin");
                }

                var isValid = decimal.TryParse(amount, out decimal amt);

                if (!isValid)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid amount");
                }
                var finalamount = amt / 100;

                decimal fee = 0;
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.AirTime && finalamount >= x.LowerBound && finalamount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };


                if (balanceResponse.Data.WithdrawableAmount < (finalamount + fee))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Insufficient fund");
                }


                var transref = $"2528{_cryptoService.GenerateNumericKey(8)}";
                var debitres = await _bankOneAccountService.Debit(finalamount, fee, accountNumber, _appSettings.DebitGL, "Bills payment", "Mobile", transref);
                if (debitres == null || debitres.Data == null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Fail to complete transaction");
                }


                var res = await _berachahThirdPartyService.SendPaymentAdvice(paymentCode, customerMobile, customerMobile, customer.Email, amount, transref);
                if (res == null || res.Data == null)
                {
                    //await _bankOneAccountService.Credit(finalamount + fee, 0, accountNumber, _appSettings.CreditGL, "Reversal", "Mobile", _cryptoService.GenerateNumericKey(12));
                    //return ResponseModel<PaymentAdviceResponse>.Failure("Transaction fail. Please try again later");
                }
                else if (!_interswtichSuccessCode.Contains(res.Data.ResponseCode))
                {
                    //await _bankOneAccountService.Credit(finalamount + fee, 0, accountNumber, _appSettings.CreditGL, "Reversal", "Mobile", _cryptoService.GenerateNumericKey(12));
                    //return ResponseModel<PaymentAdviceResponse>.Failure("Transaction fail. Please try again later");
                }

                var transaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = finalamount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    Currency = "NGN",
                    PaymentItemCode = paymentCode,
                    TransactionReference = transref,
                    TransactionType = TransactionType.AirTime,
                    ResponseCode = res?.Data?.ResponseCode ?? "99",
                    RecordType = RecordType.Debit,
                    Fee = fee,
                    PaymentReference = res?.Data?.TransactionRef,
                    Status = (res.IsSuccessful && _interswtichSuccessCode.Contains(res?.Data?.ResponseCode)) ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED

                };
                _unitOfWork.TransactionRepository.Add(transaction);
                await _unitOfWork.Complete();
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", transaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", transaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", transaction.Narration).Replace("[ACCOUNT_BALANCE]", balanceResponse.Data.WithdrawableAmount.ToString()).Replace("[TRANSACTION_TYPE]", "AirTime").Replace("[TRANSACTION_REFERENCE]", transaction.TransactionReference).Replace("[TIME]", transaction.CreatedDate.TimeOfDay.ToString()));
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<PaymentAdviceResponse>.Failure("Exception error");
            }
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> WalletAirtimePurchase(string paymentCode, string customerMobile, string amount, string pin)
        {
            try
            {

                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == _currentUser.GetCustomerId());
                if (customer is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Customer not found");
                }

                var wallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customer.Id);
                if (wallet is null)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Wallet not found");
                }

                if (!wallet.Status.Equals(WalletStatus.ACTIVE))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Account Blocked, Please contact customer care");
                }
                if (!wallet.ValidateCheckSum())
                {
                    wallet.Status = WalletStatus.INACTIVE;
                    wallet.ModifiedBy = customer.FullName;
                    wallet.ModifiedByIp = "::0";
                    wallet.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.WalletRepository.Update(wallet);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your account is temporarily blocked, Please contact customer care");
                }

                if (customer.PinTries >= 5)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Your transaction pin has been blocked");
                }

                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, pin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.FullName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid transaction pin");
                }

                var isValid = decimal.TryParse(amount, out decimal amt);

                if (!isValid)
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Invalid amount");
                }
                var finalamount = amt / 100;

                decimal fee = 0;
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.AirTime && finalamount >= x.LowerBound && finalamount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                if (wallet.Balance < (finalamount + fee))
                {
                    return ResponseModel<PaymentAdviceResponse>.Failure("Insufficient fund");
                }


                //billCustomer.Data.
                wallet.Balance -= (finalamount + fee);
                wallet.ModifiedBy = customer.FullName;
                wallet.ModifiedDate = DateTime.UtcNow;
                wallet.CheckSum = wallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(wallet);

                var transref = $"2528{_cryptoService.GenerateNumericKey(8)}";
                var res = await _berachahThirdPartyService.SendPaymentAdvice(paymentCode, customerMobile, customerMobile, customer.Email, amount, transref);
                if (!res.IsSuccessful)
                {
                    wallet.Balance += (finalamount + fee);
                    wallet.ModifiedBy = customer.FullName;
                    wallet.ModifiedDate = DateTime.UtcNow;
                    wallet.CheckSum = wallet.GetCheckSum();
                    _unitOfWork.WalletRepository.Update(wallet);
                    return ResponseModel<PaymentAdviceResponse>.Failure(res.Message);
                }
                //else if (res.Data != null && !(res.Data.ResponseCode.Contains("90000") || res.Data.ResponseCode.Contains("90010") || res.Data.ResponseCode.Contains("90011") || res.Data.ResponseCode.Contains("90016")))
                //{
                //    wallet.Balance += (finalamount + fee);
                //    wallet.ModifiedBy = customer.FullName;
                //    wallet.ModifiedDate = DateTime.UtcNow;
                //    wallet.CheckSum = wallet.GetCheckSum();
                //    _unitOfWork.WalletRepository.Update(wallet);

                //    return ResponseModel<PaymentAdviceResponse>.Failure(res.Data.ResponseMessage);
                //}

                var transaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = finalamount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    Currency = "NGN",
                    PaymentItemCode = paymentCode,
                    TransactionReference = transref,
                    TransactionType = TransactionType.AirTime,
                    ResponseCode = res?.Data?.ResponseCode ?? "99",
                    RecordType = RecordType.Debit,
                    Fee = fee,
                    PaymentReference = res?.Data?.TransactionRef,
                    Status = (res.IsSuccessful && res?.Data?.ResponseCode == "9999") ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED

                };
                _unitOfWork.TransactionRepository.Add(transaction);
                await _unitOfWork.Complete();
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _berachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", transaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", transaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", transaction.Narration).Replace("[ACCOUNT_BALANCE]", wallet.Balance.ToString()).Replace("[TRANSACTION_TYPE]", "AirTime").Replace("[TRANSACTION_REFERENCE]", transaction.TransactionReference).Replace("[TIME]", transaction.CreatedDate.TimeOfDay.ToString()));
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting customers: {ex.Message}", nameof(GetBillers));
                return ResponseModel<PaymentAdviceResponse>.Failure("Exception error");
            }
        }
    }
}
