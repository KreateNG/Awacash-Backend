using Awacash.Application.DisputeLogs.DTOs;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.DisputeLogs.Services
{
    public interface IDisputeLogService
    {
        Task<ResponseModel<bool>> CreateDisputeLogAsync(string accountNumber, string email, string phoneNumber, decimal amount, DateTime transactionDate, string comment);
        Task<ResponseModel<DisputeLogDTO>> GetDisputeLogByIdAsync(string id);
        Task<ResponseModel<List<DisputeLogDTO>>> GetAllDisputeLogAsync();
        Task<ResponseModel<PagedResult<DisputeLogDTO>>> GetPagedDisputeLogAsync();
    }
}
