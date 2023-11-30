using System;
using Awacash.Shared;
using Newtonsoft.Json;
using System.Runtime;
using TransactionQueryJob.Interfaces;

namespace TransactionQueryJob.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ILogger<CommunicationService> _logger;
        private readonly IRestSharpHelper _restSharpHelper;
        public CommunicationService(IRestSharpHelper restSharpHelper, ILogger<CommunicationService> logger)
        {
            _restSharpHelper = restSharpHelper;
            _logger = logger;
        }
        public async Task SendEmail(string to, string subject, string body)
        {
            try
            {
                var obj = new
                {
                    to,
                    subject,
                    body
                };
                var response = await _restSharpHelper.MakeRequest(obj, "https://app.berachahmfb.com/Berachahmiddleware/", "api/Communications/sendemail", RestSharp.Method.Post);
                if (response != null)
                {
                    var responseObject = JsonConvert.DeserializeObject<ResponseModel<bool>>(response.Content);

                }
                //return ResponseModel<bool>.Failure("Failed to send email, please try again");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                //return ResponseModel<bool>.Failure("Failed to send email, please try again");
            }
        }

        public Task SendSms(string phoneNumber, string message)
        {
            throw new NotImplementedException();
        }
    }
}

