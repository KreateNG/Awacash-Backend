using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Wema.Services
{
    public interface IWemaService
    {
        Task<ResponseModel<NameEnquiryResponse>> WemaAccointLookUp(string accountNumber);
    }
}
