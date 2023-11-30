
using Awacash.Domain.Models.BillsPayment;
using Awacash.Domain.Models.Customer;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Interfaces
{
    public interface IAwacashThirdPartyService
    {
        Task<ResponseModel<bool>> SendSms(string phoneNumber, string message, string accountNumber, string accountId);
        Task<ResponseModel<bool>> SendEmail(string to, string subject, string body, Dictionary<string, string>? files = null);
        Task<ResponseModel<List<NipBank>>> GetBanks();
        Task<ResponseModel<NipNameEnquiryResponse>> NipNameEnquiry(string accountNumber, string bankCode);
        Task<ResponseModel<NipTransferResponse>> NipFundsTransfer(string destinationBankCode, string destinationAccountNumber, string accountName, string sourceAccount, string originatorName, string amount, string paymentReference, string narration);

        Task<ResponseModel<List<Biller>>> GetBillers();
        Task<ResponseModel<List<Biller>>> GetBillerByCategory(int categoryId);
        Task<ResponseModel<List<Paymentitem>>> GetBillerPaymentItems(string billerId);
        Task<ResponseModel<PaymentAdviceResponse>> SendPaymentAdvice(string paymentCode, string customerId, string customerMobile, string customerEmail, string amount, string requestReference);
        Task<ResponseModel<BillPaymentCustomer>> ValidateCustomer(string customerId, string paymentCode);
        Task<ResponseModel<BillerCategory>> GetBillerCategory();
        Task<ResponseModel<BvnCustomerInfo>> GetBvnCustomerInfoWithAccessCode(string accessCode);
        Task<ResponseModel<string>> InitializeBvnAuth();
        Task<ResponseModel<TransactionStatusNotificationResponse>> TransQuery(string crcaccount);
        Task<ResponseModel<TransactionStatusNotificationResponse>> GetCollectionBanlance(string accountNumber);





    }
}
