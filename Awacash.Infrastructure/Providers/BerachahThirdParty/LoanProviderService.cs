using System;
using Awacash.Domain.Entities;
using Awacash.Domain.Interfaces;
using Awacash.Domain.Models.EasyPay;
using Awacash.Domain.Models.Loan;
using Awacash.Infrastructure.Helpers;
using Awacash.Infrastructure.Settings;
using Awacash.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Awacash.Infrastructure.Providers.BerachahThirdParty;

public class LoanProviderService : ILoanProviderService
{
    private readonly ILogger<LoanProviderService> _logger;
    private readonly IRestSharpHelper _restSharpHelper;
    private readonly BerachahThirdPartySettings _settings;
    public LoanProviderService(ILogger<LoanProviderService> logger, IRestSharpHelper restSharpHelper, IOptions<BerachahThirdPartySettings> settings)
    {
        _logger = logger;
        _restSharpHelper = restSharpHelper;
        _settings = settings.Value;
    }

    public async Task<ResponseModel<BaseLoanResponseData>> CreateLoan(CreateLoanRequest createLoanRequest)
    {
        try
        {

            var response = await _restSharpHelper.MakeRequest(createLoanRequest, _settings.BaseUrl, $"api/Loan/create-loan", RestSharp.Method.Post);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<BaseLoanResponseData>>(response.Content);
                return responseObject;
            }
            return ResponseModel<BaseLoanResponseData>.Failure("Failed to create laon, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<BaseLoanResponseData>.Failure("Failed to create laon, please try again");
        }
    }

    public async Task<ResponseModel<List<LoanBalanceModel>>> GetLoanBalance(string customerId)
    {
        try
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Loan/get-loan-balances/{customerId}", RestSharp.Method.Get);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<LoanBalanceModel>>>(response.Content);
                return responseObject;
            }
            return ResponseModel<List<LoanBalanceModel>>.Failure("Failed to send sms, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<List<LoanBalanceModel>>.Failure("Failed to get laon balances, please try again");
        }
    }

    public async Task<ResponseModel<List<LoanModel>>> GetLoanByCustomerId(string customerId)
    {
        try
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Loan/get-loan-balances/{customerId}", RestSharp.Method.Get);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<LoanModel>>>(response.Content);
                return responseObject;
            }
            return ResponseModel<List<LoanModel>>.Failure("Failed to send sms, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<List<LoanModel>>.Failure("Failed to get laon balances, please try again");
        }
    }

    public async Task<ResponseModel<List<LoanStatusModel>>> GetLoansByStatus(string status)
    {
        try
        {
            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Loan/get-status/{status}", RestSharp.Method.Get);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<List<LoanStatusModel>>>(response.Content);
                return responseObject;
            }
            return ResponseModel<List<LoanStatusModel>>.Failure("Failed to send sms, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<List<LoanStatusModel>>.Failure("Failed to get laon balances, please try again");
        }
    }

    public async Task<ResponseModel<LoanRepaymentModel>> GetLoanTotalRepayment(string accountNumber)
    {
        try
        {

            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Loan/get-total-repayment/{accountNumber}", RestSharp.Method.Get);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<LoanRepaymentModel>>(response.Content);
                return responseObject;
            }
            return ResponseModel<LoanRepaymentModel>.Failure("Failed to create laon, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<LoanRepaymentModel>.Failure("Failed to create laon, please try again");
        }
    }

    public async Task<ResponseModel<LoanRepaymentModel>> GetLoanLastRepaymentDetails(string accountNumber)
    {
        try
        {

            var response = await _restSharpHelper.MakeRequest(null, _settings.BaseUrl, $"api/Loan/get-last-repayment-details/{accountNumber}", RestSharp.Method.Post);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<LoanRepaymentModel>>(response.Content);
                return responseObject;
            }
            return ResponseModel<LoanRepaymentModel>.Failure("Failed to create laon, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<LoanRepaymentModel>.Failure("Failed to create laon, please try again");
        }
    }

    public async Task<ResponseModel<BaseLoanResponseData>> RepayLoan(LoanRepaymentRequest loanRepaymentRequest)
    {
        try
        {

            var response = await _restSharpHelper.MakeRequest(loanRepaymentRequest, _settings.BaseUrl, $"api/Loan/repay-loan", RestSharp.Method.Post);

            if (response != null && response.IsSuccessful)
            {
                var responseObject = JsonConvert.DeserializeObject<ResponseModel<BaseLoanResponseData>>(response.Content);
                return responseObject;
            }
            return ResponseModel<BaseLoanResponseData>.Failure("Failed to create laon, please try again");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return ResponseModel<BaseLoanResponseData>.Failure("Failed to create laon, please try again");
        }
    }
}

