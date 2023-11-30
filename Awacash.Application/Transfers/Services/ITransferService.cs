using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Services
{
    public interface ITransferService
    {
        Task<ResponseModel<List<NipBank>>> GetNIPBanks();
        Task<ResponseModel<NipTransferResponse>> PostInterBankTransfer(string sourceAccount, string destinationAccount, string bankCode, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary);
        Task<ResponseModel<string>> PostLocalTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary);
        Task<ResponseModel<NipNameEnquiryResponse>> PostNipNameEnquiry(string accountNumber, string bankCode);
        Task<ResponseModel<NipNameEnquiryResponse>> LocalTransferNameEnquiry(string PhoneNumber);
        Task<ResponseModel<BalanceEnquiryResponse>> GetBalance(string accountNumber);
        Task<ResponseModel<bool>> UpdateWallet();
        Task<ResponseModel<BalanceEnquiryResponse>> GetWalletBalance(string accountNumber);
        Task<ResponseModel<NipNameEnquiryResponse>> LocalWalletTransferNameEnquiry(string PhoneNumber);
        Task<ResponseModel<NipNameEnquiryResponse>> PostWalletNipNameEnquiry(string accountNumber, string bankCode);
        Task<ResponseModel<string>> PostWalletLocalTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary);
        Task<ResponseModel<NipTransferResponse>> PostWalletInterBankTransfer(string sourceAccount, string destinationAccount, string bankCode, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary);
        Task<ResponseModel<List<NipBank>>> GetWalletNIPBanks();
        Task<ResponseModel<string>> PostOwnAccountTransfer(string sourceAccount, string destinationAccount, decimal amount, string narration, string transactionPin, string transactionReference, bool addAsBeneficary);


    }
}
