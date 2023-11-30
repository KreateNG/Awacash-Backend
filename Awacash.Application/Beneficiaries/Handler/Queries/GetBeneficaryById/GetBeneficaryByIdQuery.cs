
using Awacash.Application.Beneficiaries.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Beneficiaries.Handler.Queries.GetBeneficaryById
{
    public record GetBeneficaryByIdQuery(string Id):IRequest<ResponseModel<BeneficiaryDTO>>;
}
