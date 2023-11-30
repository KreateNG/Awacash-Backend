using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Common.Constants;
using Awacash.Domain.Entities;
using Awacash.Domain.Enums;
using Awacash.Domain.Extentions;
using Awacash.Domain.Helpers;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Services
{
    public class TransferService : ITransferService
    {
        private readonly ILogger<TransferService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwacashThirdPartyService _BerachahThirdPartyService;
        private readonly ICurrentUser _currentUser;
        private readonly ICryptoService _cryptoService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEasyPayService _easyPayService;
        private readonly IBankOneAccountService _bankOneAccountService;
        public TransferService(ILogger<TransferService> logger, IUnitOfWork unitOfWork, IAwacashThirdPartyService BerachahThirdPartyService, ICurrentUser currentUser, ICryptoService cryptoService, IDateTimeProvider dateTimeProvider, IEasyPayService easyPayService, IBankOneAccountService bankOneAccountService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _BerachahThirdPartyService = BerachahThirdPartyService;
            _currentUser = currentUser;
            _cryptoService = cryptoService;
            _dateTimeProvider = dateTimeProvider;
            _easyPayService = easyPayService;
            _bankOneAccountService = bankOneAccountService;
        }

        #region Bankone section

        public async Task<ResponseModel<List<NipBank>>> GetNIPBanks()
        {
            try
            {
                return await _easyPayService.GetAllBanks();
                //await _BerachahThirdPartyService.GetBanks();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<NipBank>>.Failure("Error occure, please try again later");
            }
        }



        public async Task<ResponseModel<NipTransferResponse>> PostInterBankTransfer(string sourceAccount, string destinationAccount, string bankCode, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary)
        {
            try
            {
                decimal fee = 0;
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Customer not found");
                }

                if (sourceAccount.Equals(destinationAccount))
                {
                    return ResponseModel<NipTransferResponse>.Failure("Debit account cannot be the same as credit account");
                }

                //validate source account

                var sourceAccountNameEnquiry = await _bankOneAccountService.GetAccountDetail(sourceAccount);
                if (sourceAccountNameEnquiry == null || sourceAccountNameEnquiry.Data == null || !sourceAccountNameEnquiry.Data.IsSuccessful)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Invalid debit account number, Please try again");
                }

                var destinationAccountNameEnquiry = await _easyPayService.NameEnquriy(destinationAccount, bankCode);
                if (destinationAccountNameEnquiry == null || destinationAccountNameEnquiry.Data == null)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Credit account number not found");
                }


                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.InterBankTransfer && amount >= x.LowerBound && amount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                var sourceBalance = await _bankOneAccountService.BalanceEnquiry(sourceAccount);
                if (sourceBalance == null || sourceBalance.Data == null)
                {
                    return ResponseModel<NipTransferResponse>.Failure("network error, please try again");

                }
                //if (!sourceBalance.Data.IsSuccessful)
                //{
                //    return ResponseModel<NipTransferResponse>.Failure("network error, please try again");

                //}
                if (sourceBalance.Data.AvailableBalance < (amount + fee))
                {
                    return ResponseModel<NipTransferResponse>.Failure("Insufficient fund");
                }


                if (customer.PinTries >= 5)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Your transaction pin has been blocked");
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


                var tranRef = _cryptoService.GenerateNumericKey(12);
                var response = await _easyPayService.Transfer(destinationAccountNameEnquiry.Data.SessionID, amount, fee, bankCode, sourceAccount, destinationAccount, narration, sourceAccountNameEnquiry.Data.Name, destinationAccountNameEnquiry.Data.AccountName, tranRef, destinationAccountNameEnquiry.Data.KycLevel, destinationAccountNameEnquiry.Data.BankVerificationNumber);


                var debitTransaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = amount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    CreditAccountName = destinationAccountNameEnquiry.Data.AccountName,
                    CreditAccountNumber = destinationAccount,
                    DestinationBankCode = bankCode,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = transactionReference,
                    TransactionType = TransactionType.InterBankTransfer,
                    ResponseCode = response.Data.ResponseCode,
                    RecordType = RecordType.Debit,
                    Status = TransactionStatus.SUCCESSFUL

                };

                _unitOfWork.TransactionRepository.Add(debitTransaction);
                await _unitOfWork.Complete();

                if (!response.Data.ResponseCode.Equals("00"))
                {
                    return ResponseModel<NipTransferResponse>.Failure(response.Data.ResponseMessage);
                }


                if (addAsBeneficary)
                {
                    var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.CustomerId == customerId && x.AccountNumber == destinationAccount);
                    if (beneficary is null)
                    {
                        var newBeneficary = new Beneficiary()
                        {
                            AccountName = destinationAccountNameEnquiry.Data.AccountName,
                            AccountNumber = destinationAccount,
                            BankName = "",
                            BankCode = bankCode,
                            CreatedBy = _currentUser.Name,
                            CustomerId = _currentUser.GetCustomerId(),
                            CreatedByIp = ";;1",
                            CreatedDate = _dateTimeProvider.UtcNow,
                            Status = Status.Active,
                            BeneficaryType = BeneficaryType.InterBankTransfer
                        };

                        _unitOfWork.BeneficiaryRepository.Add(newBeneficary);
                        await _unitOfWork.Complete();
                    }
                }
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", debitTransaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", debitTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", debitTransaction.Narration).Replace("[ACCOUNT_BALANCE]", sourceBalance.Data.AvailableBalance.ToString()).Replace("[TRANSACTION_TYPE]", debitTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", debitTransaction.TransactionReference).Replace("[TIME]", debitTransaction.CreatedDate.TimeOfDay.ToString()));
                }
                return ResponseModel<NipTransferResponse>.Success(new NipTransferResponse(response.Data.ResponseCode, tranRef));
                //return response;
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<NipTransferResponse>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<string>> PostLocalTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary)
        {
            try
            {
                decimal fee = 0;
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<string>.Failure("Customer not found");
                }



                var sourceAccountNameEnquiry = await _bankOneAccountService.GetAccountDetail(sourceAccount);
                if (sourceAccountNameEnquiry == null || sourceAccountNameEnquiry.Data == null || !sourceAccountNameEnquiry.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("Invalid debit account number, Please try again");
                }


                var destinationAccountNameEnquiry = await _bankOneAccountService.GetAccountDetail(destinationAccount);
                if (destinationAccountNameEnquiry == null || destinationAccountNameEnquiry.Data == null || !destinationAccountNameEnquiry.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("Credit account number not found");
                }




                var sourceBalance = await _bankOneAccountService.BalanceEnquiry(sourceAccount);
                if (sourceBalance == null || sourceBalance.Data == null)
                {
                    return ResponseModel<string>.Failure("network error, please try again");

                }
                if (sourceBalance.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("network error, please try again");

                }
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.LocalTransfer && amount >= x.LowerBound && amount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                if (sourceBalance.Data.AvailableBalance < (amount + fee))
                {
                    return ResponseModel<string>.Failure("Insufficient fund");
                }



                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, transactionPin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.LastName = " " + customer.FirstName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<string>.Failure("Invalid transaction pin");
                }

                var tranRef = _cryptoService.GenerateNumericKey(12);

                var response = await _bankOneAccountService.IntraBankTransfer(amount + fee, sourceAccount, destinationAccount, narration, "mobile", tranRef);

                var debitTransaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = amount,
                    DebitAccountName = sourceAccountNameEnquiry.Data.Name,
                    DebitAccountNumber = sourceAccount,
                    CreditAccountName = destinationAccountNameEnquiry.Data.Name,
                    CreditAccountNumber = destinationAccount,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = tranRef,
                    TransactionType = TransactionType.LocalTransfer,
                    RecordType = RecordType.Debit,
                    Status = response.Data.IsSuccessful ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED
                };

                _unitOfWork.TransactionRepository.Add(debitTransaction);

                if (!response.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure(response.Data.ResponseMessage);
                }

                //var creditTransaction = new Transaction()
                //{
                //    CustomerId = creditAccount.Id,
                //    Amount = amount,
                //    DebitAccountName = sourceAccountNameEnquiry.Data.Name,
                //    DebitAccountNumber = sourceAccount,
                //    CreditAccountName = destinationAccountNameEnquiry.Data.Name,
                //    CreditAccountNumber = destinationAccount,
                //    Fee = fee,
                //    Narration = narration,
                //    Currency = "NGN",
                //    TransactionReference = transactionReference,
                //    TransactionType = TransactionType.LocalTransfer,
                //    RecordType = RecordType.Credit,
                //    Status = TransactionStatus.SUCCESSFUL


                //};

                //_unitOfWork.TransactionRepository.Add(creditTransaction);
                await _unitOfWork.Complete();

                if (addAsBeneficary)
                {
                    var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.CustomerId == customerId && x.AccountNumber == destinationAccount);
                    if (beneficary is null)
                    {
                        var newBeneficary = new Beneficiary()
                        {
                            AccountName = destinationAccountNameEnquiry.Data.Name,
                            AccountNumber = destinationAccount,
                            BankName = "Awacash",
                            CreatedBy = _currentUser.Name,
                            CustomerId = _currentUser.GetCustomerId(),
                            CreatedByIp = ";;1",
                            CreatedDate = _dateTimeProvider.UtcNow,
                            Status = Status.Active,
                            BeneficaryType = BeneficaryType.LocalTransfer
                        };

                        _unitOfWork.BeneficiaryRepository.Add(newBeneficary);
                        await _unitOfWork.Complete();
                    }
                }


                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {

                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", sourceAccountNameEnquiry.Data.Name).Replace("[ACCOUNT_NUMBER]", sourceAccount).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", debitTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", debitTransaction.Narration).Replace("[TRANSACTION_TYPE]", debitTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", debitTransaction.TransactionReference).Replace("[TIME]", debitTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", sourceBalance.Data.AvailableBalance.ToString()));





                    //var destmssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");

                    //_BerachahThirdPartyService.SendEmail(creditAccount.Email, emailTemplate.Subject, destmssge.Replace("[CUSTOMER_NAME]", creditAccount.FullName).Replace("[ACCOUNT_NUMBER]", creditTransaction.CreditAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("ACCOUNT_BALANCE", creditWallet.Balance.ToString()).Replace("[DATE]", creditTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", creditTransaction.Narration).Replace("[TRANSACTION_TYPE]", creditTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", creditTransaction.TransactionReference).Replace("[TIME]", creditTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", creditWallet.Balance.ToString()));

                }


                return ResponseModel<string>.Success(transactionReference);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<string>.Failure("Error occure please try again later");
            }

        }


        public async Task<ResponseModel<string>> PostOwnAccountTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary)
        {
            try
            {
                decimal fee = 0;
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<string>.Failure("Customer not found");
                }



                var sourceAccountNameEnquiry = await _bankOneAccountService.GetAccountsByAccountNumber(sourceAccount);
                if (sourceAccountNameEnquiry == null || sourceAccountNameEnquiry.Data == null || !sourceAccountNameEnquiry.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("Invalid debit account number, Please try again");
                }

                if (customer.AccountId != sourceAccountNameEnquiry.Data.CustomerID)
                {
                    return ResponseModel<string>.Failure("Invalid customer account number, Please try again");
                }

                var destinationAccountNameEnquiry = await _bankOneAccountService.GetAccountsByAccountNumber(destinationAccount);
                if (destinationAccountNameEnquiry == null || destinationAccountNameEnquiry.Data == null || !destinationAccountNameEnquiry.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("Credit account number not found");
                }


                if (!sourceAccountNameEnquiry.Data.CustomerID.Equals(destinationAccountNameEnquiry.Data.CustomerID))
                {
                    return ResponseModel<string>.Failure("Cannot transfer to other customers, Please use intra bank channel");
                }

                var sourceBalance = await _bankOneAccountService.BalanceEnquiry(sourceAccount);
                if (sourceBalance == null || sourceBalance.Data == null)
                {
                    return ResponseModel<string>.Failure("network error, please try again");

                }
                if (sourceBalance.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure("network error, please try again");

                }
                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.LocalTransfer && amount >= x.LowerBound && amount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                if (sourceBalance.Data.AvailableBalance < (amount + fee))
                {
                    return ResponseModel<string>.Failure("Insufficient fund");
                }



                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, transactionPin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.LastName = " " + customer.FirstName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<string>.Failure("Invalid transaction pin");
                }

                var tranRef = _cryptoService.GenerateNumericKey(12);

                var response = await _bankOneAccountService.IntraBankTransfer(amount + fee, sourceAccount, destinationAccount, narration, "mobile", tranRef);

                var debitTransaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = amount,
                    DebitAccountName = sourceAccountNameEnquiry.Data.Name,
                    DebitAccountNumber = sourceAccount,
                    CreditAccountName = destinationAccountNameEnquiry.Data.Name,
                    CreditAccountNumber = destinationAccount,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = tranRef,
                    TransactionType = TransactionType.LocalTransfer,
                    RecordType = RecordType.Debit,
                    Status = response.Data.IsSuccessful ? TransactionStatus.SUCCESSFUL : TransactionStatus.FAILED
                };

                _unitOfWork.TransactionRepository.Add(debitTransaction);

                if (!response.Data.IsSuccessful)
                {
                    return ResponseModel<string>.Failure(response.Data.ResponseMessage);
                }

                //var creditTransaction = new Transaction()
                //{
                //    CustomerId = creditAccount.Id,
                //    Amount = amount,
                //    DebitAccountName = sourceAccountNameEnquiry.Data.Name,
                //    DebitAccountNumber = sourceAccount,
                //    CreditAccountName = destinationAccountNameEnquiry.Data.Name,
                //    CreditAccountNumber = destinationAccount,
                //    Fee = fee,
                //    Narration = narration,
                //    Currency = "NGN",
                //    TransactionReference = transactionReference,
                //    TransactionType = TransactionType.LocalTransfer,
                //    RecordType = RecordType.Credit,
                //    Status = TransactionStatus.SUCCESSFUL


                //};

                //_unitOfWork.TransactionRepository.Add(creditTransaction);
                await _unitOfWork.Complete();

                if (addAsBeneficary)
                {
                    var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.CustomerId == customerId && x.AccountNumber == destinationAccount);
                    if (beneficary is null)
                    {
                        var newBeneficary = new Beneficiary()
                        {
                            AccountName = destinationAccountNameEnquiry.Data.Name,
                            AccountNumber = destinationAccount,
                            BankName = "Awacash",
                            CreatedBy = _currentUser.Name,
                            CustomerId = _currentUser.GetCustomerId(),
                            CreatedByIp = ";;1",
                            CreatedDate = _dateTimeProvider.UtcNow,
                            Status = Status.Active,
                            BeneficaryType = BeneficaryType.LocalTransfer
                        };

                        _unitOfWork.BeneficiaryRepository.Add(newBeneficary);
                        await _unitOfWork.Complete();
                    }
                }


                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {

                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", sourceAccountNameEnquiry.Data.Name).Replace("[ACCOUNT_NUMBER]", sourceAccount).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", debitTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", debitTransaction.Narration).Replace("[TRANSACTION_TYPE]", debitTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", debitTransaction.TransactionReference).Replace("[TIME]", debitTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", sourceBalance.Data.AvailableBalance.ToString()));





                    //var destmssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");

                    //_BerachahThirdPartyService.SendEmail(creditAccount.Email, emailTemplate.Subject, destmssge.Replace("[CUSTOMER_NAME]", creditAccount.FullName).Replace("[ACCOUNT_NUMBER]", creditTransaction.CreditAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("ACCOUNT_BALANCE", creditWallet.Balance.ToString()).Replace("[DATE]", creditTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", creditTransaction.Narration).Replace("[TRANSACTION_TYPE]", creditTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", creditTransaction.TransactionReference).Replace("[TIME]", creditTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", creditWallet.Balance.ToString()));

                }


                return ResponseModel<string>.Success(transactionReference);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<string>.Failure("Error occure please try again later");
            }

        }

        public async Task<ResponseModel<NipNameEnquiryResponse>> PostNipNameEnquiry(string accountNumber, string bankCode)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NipNameEnquiryResponse>.Failure("Customer not found");
                }

                var name = await _easyPayService.NameEnquriy(accountNumber, bankCode);
                if (name == null || name.Data == null || !name.Data.ResponseCode.Equals("00"))
                {
                    return ResponseModel<NipNameEnquiryResponse>.Failure("Account verification failed. Please check the details and try again");
                }

                return ResponseModel<NipNameEnquiryResponse>.Success(new NipNameEnquiryResponse(bankCode, name.Data.AccountName));

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NipNameEnquiryResponse>.Failure("Error occure please try again later");
            }

        }

        public async Task<ResponseModel<NipNameEnquiryResponse>> LocalTransferNameEnquiry(string accountNumber)
        {
            try
            {
                var customer = await _bankOneAccountService.GetAccountDetail(accountNumber);
                if (customer is null || customer.Data == null || !customer.Data.IsSuccessful)
                {
                    return ResponseModel<NipNameEnquiryResponse>.Failure("Account verification failed. Please check the details and try again");
                }
                return ResponseModel<NipNameEnquiryResponse>.Success(new NipNameEnquiryResponse("00", customer.Data.Name));
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<NipNameEnquiryResponse>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<BalanceEnquiryResponse>> GetBalance(string accountNumber)
        {
            try
            {
                var customer = await _bankOneAccountService.BalanceEnquiry(accountNumber);
                if (customer is null || customer.Data is null)
                {
                    return ResponseModel<BalanceEnquiryResponse>.Failure("Account not found");
                }
                return ResponseModel<BalanceEnquiryResponse>.Success(new BalanceEnquiryResponse(customer.Data.AvailableBalance, customer.Data.Name));
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<BalanceEnquiryResponse>.Failure("Error occure please try again later");
            }
        }

        #endregion


        #region Wallet Section
        public async Task<ResponseModel<NipTransferResponse>> PostWalletInterBankTransfer(string sourceAccount, string destinationAccount, string bankCode, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary)
        {
            try
            {
                decimal fee = 0;
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Customer not found");
                }

                if (sourceAccount.Equals(destinationAccount))
                {
                    return ResponseModel<NipTransferResponse>.Failure("Debit account cannot be the same as credit account");
                }

                // get debit wallet
                var debitWallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customerId && x.IsDeleted == false);
                if (debitWallet is null)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Invalid customer Please contact customer care");
                }
                if (debitWallet.Status != WalletStatus.ACTIVE)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Account Blocked, Please contact customer care");
                }
                if (!debitWallet.ValidateCheckSum())
                {
                    debitWallet.Status = WalletStatus.INACTIVE;
                    debitWallet.ModifiedBy = customer.FullName;
                    debitWallet.ModifiedByIp = "::0";
                    debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.WalletRepository.Update(debitWallet);
                    await _unitOfWork.Complete();
                    return ResponseModel<NipTransferResponse>.Failure("Your account is temporarily blocked, Please contact customer care");
                }


                if (debitWallet.Balance < amount)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Insufficient fund");
                }
                // validate destination account

                var destinationAccountDetails = await _BerachahThirdPartyService.NipNameEnquiry(destinationAccount, bankCode);
                if (!destinationAccountDetails.IsSuccessful)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Credit account number not found");
                }

                //var sourceAccountDetails = await _BerachahThirdPartyService.NipNameEnquiry(sourceAccount, bankCode);
                //if (!destinationAccountDetails.IsSuccessful)
                //{
                //    return ResponseModel<NipTransferResponse>.Failure("Debit account number not found");
                //}

                if (customer.PinTries >= 5)
                {
                    return ResponseModel<NipTransferResponse>.Failure("Your transaction pin has been blocked");
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

                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.InterBankTransfer && amount >= x.LowerBound && amount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                debitWallet.Balance -= (amount + fee);
                debitWallet.ModifiedBy = customer.FullName;
                debitWallet.ModifiedByIp = "::0";
                debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                debitWallet.CheckSum = debitWallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(debitWallet);
                await _unitOfWork.Complete();
                var response = await _BerachahThirdPartyService.NipFundsTransfer(bankCode, destinationAccount, destinationAccountDetails.Data.Name, sourceAccount, "Berarchah", amount.ToString(), transactionReference, narration);

                if (response != null)
                {
                    if (!response.IsSuccessful)
                    {
                        debitWallet.Balance += (amount + fee);
                        debitWallet.ModifiedBy = customer.FullName;
                        debitWallet.ModifiedByIp = "::0";
                        debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                        debitWallet.CheckSum = debitWallet.GetCheckSum();
                        _unitOfWork.WalletRepository.Update(debitWallet);
                        await _unitOfWork.Complete();
                        return ResponseModel<NipTransferResponse>.Failure("Fail to complete transaction, please try again later");
                    }
                    if (!response.Data.Code.Equals("00"))
                    {
                        debitWallet.Balance += (amount + fee);
                        debitWallet.ModifiedBy = customer.FullName;
                        debitWallet.ModifiedByIp = "::0";
                        debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                        debitWallet.CheckSum = debitWallet.GetCheckSum();
                        _unitOfWork.WalletRepository.Update(debitWallet);
                        await _unitOfWork.Complete();
                        return ResponseModel<NipTransferResponse>.Failure(response.Message);
                    }
                }


                var debitTransaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = amount,
                    DebitAccountName = customer.FullName,
                    DebitAccountNumber = customer.PhoneNumber,
                    CreditAccountName = destinationAccountDetails.Data.Name,
                    CreditAccountNumber = destinationAccount,
                    DestinationBankCode = bankCode,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = transactionReference,
                    TransactionType = TransactionType.InterBankTransfer,
                    ResponseCode = response.Data.Code,
                    RecordType = RecordType.Debit,
                    Status = TransactionStatus.SUCCESSFUL

                };

                _unitOfWork.TransactionRepository.Add(debitTransaction);
                await _unitOfWork.Complete();
                if (addAsBeneficary)
                {
                    var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.CustomerId == customerId && x.AccountNumber == destinationAccount);
                    if (beneficary is null)
                    {
                        var newBeneficary = new Beneficiary()
                        {
                            AccountName = destinationAccountDetails.Data.Name,
                            AccountNumber = destinationAccount,
                            BankName = "",
                            BankCode = bankCode,
                            CreatedBy = _currentUser.Name,
                            CustomerId = _currentUser.GetCustomerId(),
                            CreatedByIp = ";;1",
                            CreatedDate = _dateTimeProvider.UtcNow,
                            Status = Status.Active,
                            BeneficaryType = BeneficaryType.InterBankTransfer
                        };

                        _unitOfWork.BeneficiaryRepository.Add(newBeneficary);
                        await _unitOfWork.Complete();
                    }
                }
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {
                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", debitTransaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", debitTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", debitTransaction.Narration).Replace("[ACCOUNT_BALANCE]", debitWallet.Balance.ToString()).Replace("[TRANSACTION_TYPE]", debitTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", debitTransaction.TransactionReference).Replace("[TIME]", debitTransaction.CreatedDate.TimeOfDay.ToString()));
                }
                return response;
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<NipTransferResponse>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<string>> PostWalletLocalTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary)
        {
            try
            {
                decimal fee = 0;
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<string>.Failure("Customer not found");
                }

                if (!customer.PhoneNumber.Equals(sourceAccount))
                {
                    return ResponseModel<string>.Failure("Debit account not found");
                }
                var creditAccount = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == destinationAccount && x.IsDeleted == false);
                if (creditAccount is null)
                {
                    return ResponseModel<string>.Failure("Credit account not found");
                }

                // get debit wallet
                var debitWallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customerId && x.IsDeleted == false);
                if (debitWallet is null)
                {
                    return ResponseModel<string>.Failure("Invalid customer Please contact customer care");
                }
                if (!debitWallet.Status.Equals(WalletStatus.ACTIVE))
                {
                    return ResponseModel<string>.Failure("Account Blocked, Please contact customer care");
                }
                if (!debitWallet.ValidateCheckSum())
                {
                    debitWallet.Status = WalletStatus.INACTIVE;
                    debitWallet.ModifiedBy = customer.FullName;
                    debitWallet.ModifiedByIp = "::0";
                    debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                    _unitOfWork.WalletRepository.Update(debitWallet);
                    await _unitOfWork.Complete();
                    return ResponseModel<string>.Failure("Your account is temporarily blocked, Please contact customer care");
                }

                if (debitWallet.Balance < amount)
                {
                    return ResponseModel<string>.Failure("Insufficient fund");
                }

                //get credit wallet
                var creditWallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == creditAccount.Id && x.IsDeleted == false);
                if (creditWallet is null)
                {
                    return ResponseModel<string>.Failure("Invalid customer Please contact customer care");
                }

                //if (!creditWallet.ValidateCheckSum())
                //{
                //    return ResponseModel<string>.Failure("Supected fraud on credit account, Please contact customer care");
                //}
                //if (creditWallet.Status != "ACTIVE")
                //{
                //    return ResponseModel<string>.Failure("Account Blocked, Please contact customer care");
                //}

                //if (customer.PinTries >= 5)
                //{
                //    return ResponseModel<string>.Failure("Your transaction pin has been blocked");
                //}
                // validate customer PIN

                if (_cryptoService.ComputeSaltedHash(customer.PinSalt.Value, transactionPin) != customer.SaltedHashedPin)
                {
                    customer.PinTries += 1;
                    customer.ModifiedBy = customer.LastName = " " + customer.FirstName;
                    customer.ModifiedDate = _dateTimeProvider.UtcNow;
                    customer.ModifiedByIp = "::0";
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.Complete();
                    return ResponseModel<string>.Failure("Invalid transaction pin");
                }

                var feeConfig = await _unitOfWork.FeeConfigurationRepository.GetByAsync(x => x.TransactionType == TransactionType.LocalTransfer && amount >= x.LowerBound && amount <= x.UpperBound);
                if (feeConfig is not null)
                {
                    fee = feeConfig.Fee;
                };

                debitWallet.Balance = debitWallet.Balance - (amount + fee);
                debitWallet.ModifiedBy = customer.LastName + " " + customer.FirstName;
                debitWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                debitWallet.ModifiedByIp = "::1";
                debitWallet.CheckSum = debitWallet.GetCheckSum();
                _unitOfWork.WalletRepository.Update(debitWallet);


                creditWallet.Balance = creditWallet.Balance + amount;
                creditWallet.ModifiedBy = creditAccount.FullName;
                creditWallet.ModifiedDate = _dateTimeProvider.UtcNow;
                creditWallet.ModifiedByIp = "::1";
                creditWallet.CheckSum = creditWallet.GetCheckSum();

                _unitOfWork.WalletRepository.Update(creditWallet);

                var debitTransaction = new Transaction()
                {
                    CustomerId = customer.Id,
                    Amount = amount,
                    DebitAccountName = customer.FirstName + " " + customer.LastName,
                    DebitAccountNumber = customer.PhoneNumber,
                    CreditAccountName = creditAccount.FirstName + " " + creditAccount.LastName,
                    CreditAccountNumber = creditAccount.PhoneNumber,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = transactionReference,
                    TransactionType = TransactionType.LocalTransfer,
                    RecordType = RecordType.Debit,
                    Status = TransactionStatus.SUCCESSFUL
                };

                _unitOfWork.TransactionRepository.Add(debitTransaction);

                var creditTransaction = new Transaction()
                {
                    CustomerId = creditAccount.Id,
                    Amount = amount,
                    DebitAccountName = customer.FirstName + " " + customer.LastName,
                    DebitAccountNumber = customer.PhoneNumber,
                    CreditAccountName = creditAccount.FirstName + " " + creditAccount.LastName,
                    CreditAccountNumber = creditAccount.PhoneNumber,
                    Fee = fee,
                    Narration = narration,
                    Currency = "NGN",
                    TransactionReference = transactionReference,
                    TransactionType = TransactionType.LocalTransfer,
                    RecordType = RecordType.Credit,
                    Status = TransactionStatus.SUCCESSFUL


                };

                _unitOfWork.TransactionRepository.Add(creditTransaction);
                await _unitOfWork.Complete();

                if (addAsBeneficary)
                {
                    var beneficary = await _unitOfWork.BeneficiaryRepository.GetByAsync(x => x.CustomerId == customerId && x.AccountNumber == destinationAccount);
                    if (beneficary is null)
                    {
                        var newBeneficary = new Beneficiary()
                        {
                            AccountName = creditAccount.FullName,
                            AccountNumber = destinationAccount,
                            BankName = "Awacash",
                            CreatedBy = _currentUser.Name,
                            CustomerId = _currentUser.GetCustomerId(),
                            CreatedByIp = ";;1",
                            CreatedDate = _dateTimeProvider.UtcNow,
                            Status = Status.Active,
                            BeneficaryType = BeneficaryType.LocalTransfer
                        };

                        _unitOfWork.BeneficiaryRepository.Add(newBeneficary);
                        await _unitOfWork.Complete();
                    }
                }


                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByAsync(x => x.EmailType == EmailType.Transaction);
                if (emailTemplate != null)
                {

                    var mssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");
                    _BerachahThirdPartyService.SendEmail(customer.Email, emailTemplate.Subject, mssge.Replace("[CUSTOMER_NAME]", customer.FullName).Replace("[ACCOUNT_NUMBER]", debitTransaction.DebitAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("[DATE]", debitTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", debitTransaction.Narration).Replace("[TRANSACTION_TYPE]", debitTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", debitTransaction.TransactionReference).Replace("[TIME]", debitTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", debitWallet.Balance.ToString()));





                    var destmssge = MailTemplateHelper.GenerateMailContent(emailTemplate.Body, "email_template.html", "Awacash");

                    _BerachahThirdPartyService.SendEmail(creditAccount.Email, emailTemplate.Subject, destmssge.Replace("[CUSTOMER_NAME]", creditAccount.FullName).Replace("[ACCOUNT_NUMBER]", creditTransaction.CreditAccountNumber).Replace("[AMOUNT]", amount.ToString()).Replace("ACCOUNT_BALANCE", creditWallet.Balance.ToString()).Replace("[DATE]", creditTransaction.CreatedDate.Date.ToString()).Replace("[NARRATION]", creditTransaction.Narration).Replace("[TRANSACTION_TYPE]", creditTransaction.RecordType == RecordType.Credit ? "Credit" : "Debit").Replace("[TRANSACTION_REFERENCE]", creditTransaction.TransactionReference).Replace("[TIME]", creditTransaction.CreatedDate.TimeOfDay.ToString()).Replace("[ACCOUNT_BALANCE]", creditWallet.Balance.ToString()));

                }


                return ResponseModel<string>.Success(transactionReference);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<string>.Failure("Error occure please try again later");
            }

        }

        public async Task<ResponseModel<NipNameEnquiryResponse>> PostWalletNipNameEnquiry(string accountNumber, string bankCode)
        {
            try
            {
                var customerId = _currentUser.GetCustomerId();
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.Id == customerId && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NipNameEnquiryResponse>.Failure("Customer not found");
                }

                return await _BerachahThirdPartyService.NipNameEnquiry(accountNumber, bankCode);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NipNameEnquiryResponse>.Failure("Error occure please try again later");
            }

        }


        public async Task<ResponseModel<NipNameEnquiryResponse>> LocalWalletTransferNameEnquiry(string PhoneNumber)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == PhoneNumber && x.IsDeleted == false);
                if (customer is null)
                {
                    return ResponseModel<NipNameEnquiryResponse>.Failure("Customer not found");
                }
                return ResponseModel<NipNameEnquiryResponse>.Success(new NipNameEnquiryResponse("00", customer.FullName));
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<NipNameEnquiryResponse>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<BalanceEnquiryResponse>> GetWalletBalance(string accountNumber)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByAsync(x => x.PhoneNumber == accountNumber || x.AccountNumber == accountNumber && x.IsDeleted == false, x => x.Wallet);
                if (customer is null)
                {
                    return ResponseModel<BalanceEnquiryResponse>.Failure("Account not found");
                }
                return ResponseModel<BalanceEnquiryResponse>.Success(new BalanceEnquiryResponse(customer.Wallet.Balance.Value, customer.FullName));
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
                return ResponseModel<BalanceEnquiryResponse>.Failure("Error occure please try again later");
            }
        }

        public async Task<ResponseModel<bool>> UpdateWallet()
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.ListAllAsync();
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        var wallet = await _unitOfWork.WalletRepository.GetByAsync(x => x.CustomerId == customer.Id);
                        if (wallet is not null)
                        {
                            wallet.FirstName = customer.FirstName;
                            wallet.LastName = customer.LastName;
                            wallet.PhoneNumber = customer.PhoneNumber;
                            _unitOfWork.WalletRepository.Update(wallet);

                            await _unitOfWork.Complete();
                            wallet.CheckSum = wallet.GetCheckSum();


                            _unitOfWork.WalletRepository.Update(wallet);
                            await _unitOfWork.Complete();
                        }
                    }
                }
                return ResponseModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ResponseModel<bool>.Failure();
            }
        }

        public async Task<ResponseModel<List<NipBank>>> GetWalletNIPBanks()
        {
            try
            {
                //return await _easyPayService.GetAllBanks();
                return await _BerachahThirdPartyService.GetBanks();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<NipBank>>.Failure("Error occure, please try again later");
            }
        }

        #endregion
    }
}
