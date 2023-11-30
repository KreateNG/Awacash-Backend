using Awacash.Domain.Enums;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.FeeConfigurations.Handler.Queries.GetFee
{
    public record GetFeeQuery(TransactionType TransactionType, decimal Amount):IRequest<ResponseModel<decimal>>;
}
