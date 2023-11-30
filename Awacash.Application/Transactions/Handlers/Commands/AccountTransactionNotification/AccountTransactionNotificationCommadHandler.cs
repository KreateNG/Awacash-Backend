using Awacash.Application.Transactions.Services;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Commands.AccountTransactionNotification
{
    public class AccountTransactionNotificationCommadHandler : IRequestHandler<AccountTransactionNotificationCommad, ResponseModel<string>>
    {
        private readonly ITransactionService _transactionService;

        public AccountTransactionNotificationCommadHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<ResponseModel<string>> Handle(AccountTransactionNotificationCommad request, CancellationToken cancellationToken)
        {
            return await _transactionService.SaveTransactionNotification(request.Originatoraccountnumber, request.Originatorname, request.Amount, request.Craccountname, request.Craccount, request.Paymentreference, request.Bankname, request.Bankcode, request.Sessionid, request.Narration);
        }
    }
}
