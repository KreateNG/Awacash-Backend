

using System.Security.Principal;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.BillsPayment;
using Awacash.Domain.Models.Customer;
using Awacash.Domain.Models.Transactions;
using Awacash.Infrastructure.Helpers;
using Awacash.Infrastructure.Settings;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Awacash.Infrastructure.Providers.BerachahThirdParty
{
    public class BerachahThirdPartyService : IAwacashThirdPartyService
    {
        private readonly ILogger<BerachahThirdPartyService> _logger;
        private readonly IRestSharpHelper _restSharpHelper;
        private readonly BerachahThirdPartySettings _settings;
        public BerachahThirdPartyService(ILogger<BerachahThirdPartyService> logger, IRestSharpHelper restSharpHelper, IOptions<BerachahThirdPartySettings> settings)
        {
            _logger = logger;
            _restSharpHelper = restSharpHelper;
            _settings = settings.Value;
        }
        //public async Task<ResponseModel<bool>> SendSms(string phoneNumber, string message)
        //{
        //    try
        //    {
        //        var obj = new
        //        {
        //            phoneNumber,
        //            message
        //        };
        //        var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/Communications/sendSms", RestSharp.Method.Post);
        //        if (response != null)
        //        {
        //            var responseObject = JsonConvert.DeserializeObject<ResponseModel<bool>>(response.Content);
        //            return responseObject;
        //        }
        //        return ResponseModel<bool>.Failure("Failed to send sms, please try again");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical(ex.Message);
        //        return ResponseModel<bool>.Failure("Failed to send sms, please try again");
        //    }
        //}

        public async Task<ResponseModel<bool>> SendSms(string phoneNumber, string message, string accountNumber, string accountId)
        {
            try
            {
                var obj = new[] {
                    new {
                        to = phoneNumber,
                        body = message,
                        accountId,
                        accountNumber
                    }
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/CoreBanking/send-sms", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<bool>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<bool>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<bool>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<List<NipBank>>> GetBanks()
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, "/api/Transactions/get-banks", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<NipBank>>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<List<NipBank>>.Failure("Failed to fetch bank, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<NipBank>>.Failure("Error occure, please try again later");
            }
        }


        public async Task<ResponseModel<NipNameEnquiryResponse>> NipNameEnquiry(string accountNumber, string bankCode)
        {
            try
            {
                var obj = new
                {
                    BankCode = bankCode,
                    AccountNumber = accountNumber,
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "/api/Transactions/name-enquiry", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<NipNameEnquiryResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<NipNameEnquiryResponse>.Failure("Failed to verify account. Please check the details or try again later");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NipNameEnquiryResponse>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<NipTransferResponse>> NipFundsTransfer(string destinationBankCode, string destinationAccountNumber, string accountName, string sourceAccount, string originatorName, string amount, string paymentReference, string narration)
        {
            try
            {
                var obj = new
                {
                    destinationBankCode,
                    destinationAccountNumber,
                    accountName,
                    sourceAccount,
                    originatorName,
                    amount,
                    paymentReference,
                    narration
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "/api/Transactions/nip-transfer", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<NipTransferResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<NipTransferResponse>.Failure("Transaction failed. please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NipTransferResponse>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<List<Biller>>> GetBillers()
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, "/api/BillPayments/get-billers", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<Biller>>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<List<Biller>>.Failure("Failed to fetch biller, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<Biller>>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<BillerCategory>> GetBillerCategory()
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, "/api/BillPayments/get-biller/category", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<BillerCategory>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<BillerCategory>.Failure("Failed to fetch biller, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<BillerCategory>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<List<Biller>>> GetBillerByCategory(int categoryId)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"/api/BillPayments/get-billers-by-category/{categoryId}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<Biller>>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<List<Biller>>.Failure("Failed to fetch biller, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<Biller>>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<List<Paymentitem>>> GetBillerPaymentItems(string billerId)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"/api/BillPayments/get-biller-payment-items/{billerId}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<Paymentitem>>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<List<Paymentitem>>.Failure("Failed to fetch payment items, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<Paymentitem>>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<PaymentAdviceResponse>> SendPaymentAdvice(string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string requestReference)
        {
            try
            {
                var obj = new
                {
                    paymentCode,
                    customerId,
                    customerMobile,
                    customerEmail,
                    amount,
                    requestReference
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "/api/BillPayments/send-payment-advice", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<PaymentAdviceResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<PaymentAdviceResponse>.Failure("Transaction failed. please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<PaymentAdviceResponse>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<BillPaymentCustomer>> ValidateCustomer(string customerId, string paymentCode)
        {
            try
            {
                var obj = new
                {
                    paymentCode,
                    customerId
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "/api/BillPayments/validate-customer", RestSharp.Method.Post);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<ResponseModel<BillPaymentCustomer>>(response.Content);
                }

                return ResponseModel<BillPaymentCustomer>.Failure("Failed to verify customer. Please check the details or try again later");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<BillPaymentCustomer>.Failure("Error occure, please try again later");
            }
        }


        public async Task<ResponseModel<string>> InitializeBvnAuth()
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"/api/Accounts/initialize-bvn-validation", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<string>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<string>.Failure("Failed to initialize bnv auth");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<string>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<BvnCustomerInfo>> GetBvnCustomerInfoWithAccessCode(string accessCode)
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"/api/Accounts/customer-info-with-bvn/{accessCode}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<BvnCustomerInfo>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<BvnCustomerInfo>.Failure("Failed to bnv access token");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<BvnCustomerInfo>.Failure("Error occure, please try again later");
            }
        }

        public async Task<ResponseModel<bool>> SendEmail(string to, string subject, string body, Dictionary<string, string>? files)
        {
            try
            {
                var obj = new
                {
                    to,
                    subject,
                    body,
                    attachments = files
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, "api/Communications/sendemail", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<bool>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<bool>.Failure("Failed to send email, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<bool>.Failure("Failed to send email, please try again");
            }
        }

        public async Task<ResponseModel<TransactionStatusNotificationResponse>> TransQuery(string accountNumber)
        {
            try
            {
                var obj = new
                {
                    craccount = accountNumber
                };

                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/Transactions/query-transaction/{accountNumber}", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<TransactionStatusNotificationResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<TransactionStatusNotificationResponse>.Failure("Failed query transaction, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<TransactionStatusNotificationResponse>.Failure("Failed query transaction, please try again");

            }
        }
        public async Task<ResponseModel<TransactionStatusNotificationResponse>> GetCollectionBanlance(string accountNumber)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Transactions/balance/23456787", RestSharp.Method.Get);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<TransactionStatusNotificationResponse>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<TransactionStatusNotificationResponse>.Failure("Failed query transaction, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<TransactionStatusNotificationResponse>.Failure("Failed query transaction, please try again");

            }
        }

    }
}
