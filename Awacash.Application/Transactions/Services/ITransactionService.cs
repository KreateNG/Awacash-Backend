using Awacash.Application.Transactions.DTOs;
using Awacash.Application.Transactions.FilterModels;
using Awacash.Domain.Models.BankOneAccount;
using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Services
{
    public interface ITransactionService
    {
        //Task<ResponseModel<List<NipBank>>> GetTNIPBanks();
        Task<ResponseModel<List<TransactionDTO>>> GetCustomerTransactions();
        Task<ResponseModel<PagedResult<TransactionDTO>>> GetPaginatedTransactions(TransactionFilterModel filter);
        Task<ResponseModel<TransactionDTO>> GetTransactionById(string id);
        Task<ResponseModel<string>> SaveTransactionNotification(string originatoraccountnumber, string originatorname, string amount, string craccountname, string craccount, string paymentreference, string bankname, string bankcode, string sessionid, string narration);

        Task<ResponseModel<List<TransactionQuery>>> TransQuery(string crcaccount);
        Task<ResponseModel<WalletBalanceDto>> GetWalletBalance();
        Task<ResponseModel<List<TransactionResponseDto>>> GetCoreTransactions(string accountNumber, DateTime startDate, DateTime endDate);

    }
}
