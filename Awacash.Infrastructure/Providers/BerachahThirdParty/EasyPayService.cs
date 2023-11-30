using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.EasyPay;
using Awacash.Domain.Models.Transactions;
using Awacash.Infrastructure.Helpers;
using Awacash.Infrastructure.Settings;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Awacash.Infrastructure.Providers.BerachahThirdParty
{
    public class EasyPayService : IEasyPayService
    {
        private readonly ILogger<EasyPayService> _logger;
        private readonly IRestSharpHelper _restSharpHelper;
        private readonly BerachahThirdPartySettings _settings;

        public EasyPayService(ILogger<EasyPayService> logger, IRestSharpHelper restSharpHelper, IOptions<BerachahThirdPartySettings> settings)
        {
            _logger = logger;
            _restSharpHelper = restSharpHelper;
            _settings = settings.Value;
        }

        public async Task<ResponseModel<BalanceEnquiryDto>> BalanceEnquriy(string accountNumber, string bankCode)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/EasyPay/balance-enquiry/{accountNumber}/{bankCode}", RestSharp.Method.Get);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<BalanceEnquiryDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<BalanceEnquiryDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<BalanceEnquiryDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<List<NipBank>>> GetAllBanks()
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, "api/EasyPay/get-banks", RestSharp.Method.Get);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<NipBank>>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<List<NipBank>>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<List<NipBank>>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<NipBank>> GetAllBanksByAccountNumber(string accountNumber)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/EasyPay/get-banks/{accountNumber}", RestSharp.Method.Get);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<NipBank>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<NipBank>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NipBank>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<TransactionStatusResponseDto>> GetTransactionStatus(string transactionID)
        {
            try
            {

                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/EasyPay/transaction-status/{transactionID}", RestSharp.Method.Get);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<TransactionStatusResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<TransactionStatusResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<TransactionStatusResponseDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<NameEnqiuryDto>> NameEnquriy(string accountNumber, string bankCode)
        {
            try
            {
                var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/EasyPay/name-enquiry/{accountNumber}/{bankCode}", RestSharp.Method.Get);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<NameEnqiuryDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<NameEnqiuryDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<NameEnqiuryDto>.Failure("Failed to send sms, please try again");
            }
        }

        public async Task<ResponseModel<TransferResponseDto>> Transfer(string nameEnquirySessionID, decimal tranAmount, decimal chargeAmount, string destBankCode, string sourceAccountNo, string destAccountNo, string narration, string senderName, string receiverName, string paymentRef, int beneficiaryKyc, string beneficiaryBvn)
        {
            try
            {
                var obj = new
                {
                    nameEnquirySessionID,
                    tranAmount,
                    chargeAmount,
                    destBankCode,
                    sourceAccountNo,
                    destAccountNo,
                    narration,
                    senderName,
                    receiverName,
                    paymentRef,
                    beneficiaryKyc,
                    beneficiaryBvn
                };
                var response = await _restSharpHelper.MakeRequest(obj, _settings.BaseUrl, $"api/EasyPay/transfer", RestSharp.Method.Post);

                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<TransferResponseDto>>(response.Content);
                    return responseObject;
                }
                return ResponseModel<TransferResponseDto>.Failure("Failed to send sms, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return ResponseModel<TransferResponseDto>.Failure("Failed to send sms, please try again");
            }
        }
    }
}

