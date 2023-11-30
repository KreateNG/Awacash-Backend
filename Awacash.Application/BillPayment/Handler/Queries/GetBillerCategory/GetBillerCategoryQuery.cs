using Awacash.Domain.Models.BillsPayment;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.BillPayment.Handler.Queries.GetBillerCategory
{
    public record GetBillerCategoryQuery(int CategoryId) : IRequest<ResponseModel<List<Biller>>>;
}
