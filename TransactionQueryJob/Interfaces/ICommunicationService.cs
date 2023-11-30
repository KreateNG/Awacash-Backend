using System;
using Awacash.Shared;

namespace TransactionQueryJob.Interfaces
{
    public interface ICommunicationService
    {
        Task SendSms(string phoneNumber, string message);
        Task SendEmail(string to, string subject, string body);
    }
}

