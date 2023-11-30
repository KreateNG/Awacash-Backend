using Awacash.Application.Wema.Services;
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
    public class AccountLookUpQuerryHandler : IRequestHandler<AccountLookUpQuerry, ResponseModel<NameEnquiryResponse>>
    {
        private readonly IWemaService _wemaService;

        public AccountLookUpQuerryHandler(IWemaService wemaService)
        {
            _wemaService = wemaService;
        }

        public Task<ResponseModel<NameEnquiryResponse>> Handle(AccountLookUpQuerry request, CancellationToken cancellationToken)
        {
            return _wemaService.WemaAccointLookUp(request.AccountNumber);
        }
    }
}
