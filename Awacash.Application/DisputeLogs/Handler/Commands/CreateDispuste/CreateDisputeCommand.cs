using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.DisputeLogs.Handler.Commands.CreateDispuste
{
    public record CreateDisputeCommand(string AccountNumber, string Email, string PhoneNumber, decimal Amount, DateTime TransactionDate, string Comment):IRequest<ResponseModel<bool>>;
}
