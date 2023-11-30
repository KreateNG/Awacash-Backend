using System;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Domain.Models.EasyPay;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;

namespace Awacash.Domain.Interfaces
{
    public interface IEasyPayService
    {
        Task<ResponseModel<List<NipBank>>> GetAllBanks();
        Task<ResponseModel<NipBank>> GetAllBanksByAccountNumber(string accountNumber);
        Task<ResponseModel<NameEnqiuryDto>> NameEnquriy(string accountNumber, string bankCode);
        Task<ResponseModel<BalanceEnquiryDto>> BalanceEnquriy(string accountNumber, string bankCode);
        Task<ResponseModel<TransferResponseDto>> Transfer(string nameEnquirySessionID, decimal tranAmount, decimal chargeAmount, string destBankCode, string sourceAccountNo, string destAccountNo, string narration, string senderName, string receiverName, string paymentRef, int beneficiaryKyc, string beneficiaryBvn);

        Task<ResponseModel<TransactionStatusResponseDto>> GetTransactionStatus(string transactionID);


    }
}

