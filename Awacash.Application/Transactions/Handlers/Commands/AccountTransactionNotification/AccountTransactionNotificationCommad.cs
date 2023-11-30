using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transactions.Handlers.Commands.AccountTransactionNotification
{
    public record AccountTransactionNotificationCommad(string Originatoraccountnumber, string Originatorname, string Amount, string Craccountname, string Craccount, string Paymentreference, string Bankname, string Bankcode, string Sessionid, string Narration) :IRequest<ResponseModel<string>>;
}
