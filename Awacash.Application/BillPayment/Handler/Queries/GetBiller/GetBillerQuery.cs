using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Queries.GetBiller
{
    public record GetBillerQuery() : IRequest<ResponseModel<BillerCategory>>;
}
