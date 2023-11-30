using Awacash.Domain.Models.Transactions;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Transfers.Handlers.Commands.NipNameEnquiry
{
    public record NipNameEnquiryCommand(string BankCode, string AccountNumber) : IRequest<ResponseModel<NipNameEnquiryResponse>>;
}
