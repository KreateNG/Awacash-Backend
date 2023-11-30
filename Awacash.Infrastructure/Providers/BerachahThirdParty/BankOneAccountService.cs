using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using Awacash.Application.Customers.DTOs;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Infrastructure.Helpers;
using Awacash.Infrastructure.Settings;
using Awacash.Shared;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Awacash.Infrastructure.Providers.BerachahThirdParty
{
    public class BankOneAccountService : IBankOneAccountService
    {
        private readonly ILogger<BankOneAccountService> _logger;
        private readonly IRestSharpHelper _restSharpHelper;
        private readonly BerachahThirdPartySettings _settings;

        public BankOneAccountService(ILogger<BankOneAccountService> logger, IRestSharpHelper restSharpHelper, IOptions<BerachahThirdPartySettings> settings)
        {
            _logger = logger;
            _restSharpHelper = restSharpHelper;
            _settings = settings.Value;
        }

        public async Task<ResponseModel<AccountOpeningResponseDto>> AccountOpening(string firstName, string lastName, string middleName, string bvn, string mobile, string gender, string placeOfBirth, string dob, string address, string nin, string email, string referralPhoneNo, string referralName, string nextOfKinPhoneNo, string nextOfKinName)
        {
            try
            {
                var obj = new
                {
                    firstName,
                    lastName,
                    middleName,
                    bvn,
                    mobile,
                    gender,
                    placeOfBirth,
                    dob,
                    address,
                    nin,
                    email,
                    referralPhoneNo,
                    referralName,
                    nextOfKinPhoneNo,
                    nextOfKinName
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/AccountOpening/account-opening", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<AccountOpeningResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<AccountOpeningResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<AccountOpeningResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<AddAccountResponseDto>> AddAccount(string customerID, string productCode, string email, string accountName, string bvn)
        {
            try
            {
                var obj = new
                {
                    customerID,
                    productCode,
                    email,
                    bvn,
                    accountName
                };

                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/accountOpening/add-account", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<AddAccountResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<AddAccountResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<AddAccountResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<BalanceEnquiryResponse>> BalanceEnquiry(string accountNumber)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/CoreBanking/balance-enquiry/{accountNumber}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<BalanceEnquiryResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<BalanceEnquiryResponse>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<BalanceEnquiryResponse>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<CreateFixedDepositeResponseDto>> CreateFixDeposit(string customerID, bool isDiscountDeposit, int interestRate, string amount, int tenure, string liquidationAccount, bool applyInterestMonthly, bool applyInterestOnRollOver, bool shouldRollOver)
        {
            try
            {
                var obj = new
                {
                    customerID,
                    isDiscountDeposit,
                    interestRate,
                    amount,
                    tenure,
                    liquidationAccount,
                    applyInterestMonthly,
                    applyInterestOnRollOver,
                    shouldRollOver
                };

                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/accountOpening/create-fixed-deposit", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<CreateFixedDepositeResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<CreateFixedDepositeResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<CreateFixedDepositeResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<FixedDepositLiquidation>> FixDepositLiquidation(string accountNumber, int liquidationType, string accountOpenningTrackingRef, string narration)
        {
            try
            {
                var obj = new
                {
                    accountNumber,
                    liquidationType,
                    accountOpenningTrackingRef,
                    narration
                };

                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/accountOpening/fixed-deposit-liquidation", RestSharp.Method.Post);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<FixedDepositLiquidation>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<FixedDepositLiquidation>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<FixedDepositLiquidation>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<CustomerAccountsResponseDto>> GetAccountsByAccountNumber(string accountNumber)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/accountOpening/get-accounts-by-accountnumber/{accountNumber}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<CustomerAccountsResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<CustomerAccountsResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<CustomerAccountsResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<AccountDetailsResponse>> GetAccountDetail(string accountNumber)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/CoreBanking/get-account-details/{accountNumber}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<AccountDetailsResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<AccountDetailsResponse>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<AccountDetailsResponse>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<AccountByBvnResponsedDto>> GetAccountsByBvn(string bvn)
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/accountOpening/get-accounts-by-bvn/{bvn}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<AccountByBvnResponsedDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<AccountByBvnResponsedDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<AccountByBvnResponsedDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<CustomerAccountsResponseDto>> GetAccountsByCustomerId(string customerId)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/accountOpening/get-accounts-by-customerId/{customerId}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<CustomerAccountsResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<CustomerAccountsResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<CustomerAccountsResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<FixedDepositeDetailResponseDto>> GetFixDepositByAccountNumber(string accountNumber)
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/accountOpening/get-fixeddeposit-by-accountNo/{accountNumber}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<FixedDepositeDetailResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<FixedDepositeDetailResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<FixedDepositeDetailResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<FixedDepositeDetailResponseDto>> GetFixDepositByPhoneNumber(string phone)
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/accountOpening/get-fixeddeposit-by-phoneNo/{phone}", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<FixedDepositeDetailResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<FixedDepositeDetailResponseDto>.Failure("error, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<FixedDepositeDetailResponseDto>.Failure("error occure, please try again");
            }
        }

        public async Task<ResponseModel<IntraBankTransferResponse>> IntraBankTransfer(decimal amount, string fromAccount, string destinationAccount, string narration, string channel, string transactionReference)
        {
            try
            {
                var obj = new
                {
                    amount,
                    fromAccount,
                    destinationAccount,
                    narration,
                    channel,
                    transactionReference
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/CoreBanking/intrabank-transfer", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<IntraBankTransferResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to complete transfer, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to complete transfer, please try again");
            }
        }

        public async Task<ResponseModel<UpdateAccountDocumentResponseDto>> UpdateAccountDocument(string accountNumber, string customerImage, string customerSignature)
        {
            try
            {
                var obj = new
                {
                    accountNumber,
                    customerImage,
                    customerSignature
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/accountOpening/update-account-document", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<UpdateAccountDocumentResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<UpdateAccountDocumentResponseDto>.Failure("Failed to update account, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<UpdateAccountDocumentResponseDto>.Failure("Failed to update account, please try again");
            }
        }

        public async Task<ResponseModel<UpdateCustomerResponseDto>> UpdateCustomer(string customerId, string bankVerificationNumber)
        {
            try
            {
                var obj = new
                {
                    customerId,
                    bankVerificationNumber
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/accountOpening/update-customers", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<UpdateCustomerResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<UpdateCustomerResponseDto>.Failure("Failed to update customer, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<UpdateCustomerResponseDto>.Failure("Failed to update customer, please try again");
            }
        }

        public async Task<ResponseModel<IntraBankTransferResponse>> Debit(decimal amount, decimal chargeAmount, string fromAccount, string gl, string narration, string channel, string transactionReference)
        {
            try
            {
                var obj = new
                {
                    chargeAmount,
                    amount,
                    fromAccount,
                    narration,
                    channel,
                    gl,
                    transactionReference
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/CoreBanking/intrabank-transfer-debit", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<IntraBankTransferResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to debit account, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to debit account, please try again");
            }
        }

        public async Task<ResponseModel<IntraBankTransferResponse>> Credit(decimal amount, decimal chargeAmount, string toAccount, string gl, string narration, string channel, string transactionReference)
        {
            try
            {
                var obj = new
                {
                    chargeAmount,
                    amount,
                    toAccount,
                    narration,
                    channel,
                    gl,
                    transactionReference
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/CoreBanking/intrabank-transfer-credit", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<IntraBankTransferResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to credit account, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<IntraBankTransferResponse>.Failure("Failed to credit account, please try again");
            }
        }

        public async Task<ResponseModel<AccountPNDResponse>> ActivatePND(string accountNumber)
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/CoreBanking/activate-pnd/{accountNumber}", RestSharp.Method.Get);
            if (response != null)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<AccountPNDResponse>>(response.Content);
                return responseObject;
            }
            return ResponseModel<AccountPNDResponse>.Failure("error, please try again");
        }


        public async Task<ResponseModel<StatementResponse>> RequestStatement(string accountNumber, DateTime from, DateTime to)
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/AccountOpening/generate-account-statement/{accountNumber}/{from.ToString("yyyy-MM-dd HH:mm:ss")}/{to.ToString("yyyy-MM-dd HH:mm:ss")}", RestSharp.Method.Get);
            if (response != null)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<StatementResponse>>(response.Content);
                return responseObject;
            }
            return ResponseModel<StatementResponse>.Failure("error, please try again");
        }

        public async Task<ResponseModel<List<TransactionResponseDto>>> GetTransactions(string accountNumber, DateTime from, DateTime to)
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/AccountOpening/get-account-transactions/{accountNumber}/{from.ToString("yyyy-MM-dd HH:mm:ss")}/{to.ToString("yyyy-MM-dd HH:mm:ss")}", RestSharp.Method.Get);
            if (response != null)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<TransactionResponseDto>>>(response.Content);
                return responseObject;

            }
            return ResponseModel<List<TransactionResponseDto>>.Failure("error, please try again");
        }

    }
}

