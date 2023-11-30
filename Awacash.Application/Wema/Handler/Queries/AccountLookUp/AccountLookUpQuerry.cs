using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Wema.Handler.Queries.AccountLookUp
{
    public record AccountLookUpQuerry(string AccountNumber):IRequest<ResponseModel<NameEnquiryResponse>>;
    
}
